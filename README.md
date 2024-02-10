# ImageCropper
**тестовое задание на ASP.NET Core 8.0, React и TypeScript**

## Клиент | React, TypeScript
![1](https://github.com/gravityLies/ImageCropper/assets/52885982/cebd8f00-bb55-4376-9327-d741bc50a5f0)

через форму выбирается картинка требуемого изображения и количество разрезов по горизонтали и вертикали 

размер изображения и адрес сервера настраиваются через `frontend\src\config.json`:
```json
{
  "serverUrl": "https://address:ports/",
  "sizeX": 1600,
  "sizeY": 1200
}
```

## Сервер | ASP.NET Core
на основе запроса сервер обрезает оригинальное изображение на нужное количество новых и отрисосывает координаты в левом верхнем углу

результат - массив объектов CropResponse:
```cs
public class CropResponse
{
    // обрезанное изображение
    public byte[] ImageData { get; set; }

    // координаты
    public int X { get; set; } 
    public int Y { get; set; }

    // размер
    public int Height { get; set; }
    public int Width { get; set; }
}
```

### Пример вывода
оригинал 1600х1200, обрезать на 4х3 картинок
![2](https://github.com/gravityLies/ImageCropper/assets/52885982/fd77b37c-4e27-4be1-a8c6-72079cb8a782)

оригинал 1600х1200, обрезать на 10х10 картинок
![3](https://github.com/gravityLies/ImageCropper/assets/52885982/b09ca777-f607-4358-b15a-b89da26c20c2)
