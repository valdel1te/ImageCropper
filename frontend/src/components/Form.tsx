import React, {ChangeEvent, FormEvent, useState} from "react";
import config from "../config.json";

interface CropResponse {
    imageData: ArrayBuffer
    x: number
    y: number
    height: number
    width: number
}

export function Form() {
    const cropRequest = new FormData()
    const [cropResponses, setCropResponses] = useState<CropResponse[]>([]);

    const handleNumberChange = (e: ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        if (cropRequest.has(name))
            cropRequest.set(name, value);
        else
            cropRequest.append(name, value);
    }

    const handleImageChange = async (e: ChangeEvent<HTMLInputElement>) => {
        const files = e.target.files;
        if (files && files.length > 0) {
            const file = files[0];

            const image = new Image();
            const imageUrl = URL.createObjectURL(file);
            image.onload = function () {
                if (image.width === config.sizeX && image.height === config.sizeY) {
                    cropRequest.append('ImageData', files[0])
                } else {
                    e.target.value = '';
                    alert(`Error: image must be ${config.sizeX}x${config.sizeY}`);
                }
                URL.revokeObjectURL(imageUrl);
            };
            image.src = imageUrl;
        }
    };

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (!cropRequest.has('ImageData')) return;

        console.log("process started >> ", e.target);

        const serverUrl = config.serverUrl + "api/crop/cropped-array"

        try {
            const response = await fetch(serverUrl, {
                method: 'POST',
                body: cropRequest
            });
            if (!response.ok) {
                throw new Error("error >> Bad response");
            }

            const cropResponse: CropResponse[] = await response.json();
            setCropResponses(cropResponse);

        } catch (error) {
            console.error("error >> ", error);
        }
    }

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <label>image source: </label>
                <input
                    id="input-image"
                    name="ImageData"
                    type="file"
                    accept="image/*"
                    onChange={handleImageChange}
                />
                <label>divide original image by I*J images: </label>
                <input
                    className="cropProps"
                    name="I"
                    type="number"
                    min="0"
                    placeholder="I"
                    onChange={handleNumberChange}
                />
                <input
                    className="cropProps"
                    name="J"
                    type="number"
                    min="0"
                    placeholder="J"
                    onChange={handleNumberChange}
                />
                <br/><br/>
                <button type="submit">
                    Get cropped I*J images
                </button>
            </form>
            {cropResponses && (
                <div id="output-list">
                    {cropResponses.map((response) => (
                        <img src={`data:image/png;base64,${response.imageData}`}
                             style={{height: response.height, width: response.width}}/>
                    ))}
                </div>
            )}
        </div>
    );
}