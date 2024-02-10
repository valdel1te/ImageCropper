import React from 'react';
import './styles.css'
import {Form} from "./components/Form";
import config from "./config.json"

function App() {
    return (
        <div id="container">
            <h1>🔭 Image Cropper</h1>
            <h3>only for {`${config.sizeX}x${config.sizeY}`} images</h3>
            <Form />
        </div>
    );
}

export default App;
