# GMILanguage
GMILanguage - интерпретируемый язык программирования исполнителя на двумерной сетке для обучения учащихся программированию.

GMI расшифровывается как - Grid Master Interpretable

Написано на C# 12(.NET 8) и Python 3.10

## Видеоролик, демонстрирующий функционирование программного продукта
Ссылка на видео: https://drive.google.com/file/d/1c4Jl3i-fo5SkNIU4SKr3CdWCix13aOeo/view?usp=sharing

## Инструкция по сборке среды разработки и выполнения для РАЗРАБОТЧИКОВ

ВНИМАНИЕ! Для запуска и сборки среды разработки и выполнения требуется установленная платформа Python версии 3.10. На данный момент поддерживается только платформа Windows 10 и выше

Скачать Python 3.10: https://www.python.org/downloads/release/python-3100/

Для сборки .py файла в .exe мы использовали программу auto-py-to-exe,
она имеет удобный интерфейс и легка во освоении. программа составляет команду в консоль используя pyinstaller, 
нам достаточно указать тип приложения, сборку в папку и путь к .py файлу, 
дальше всё происходит автоматически, на выход получаем папку с .exe файлом и папкой, автоматически составленной программой.
в коде среды разработки предполагается, что все файлы в одной директории,
так  что в эту папку надо добавить иконку приложения,  картинку робота, 
.ui  файл среды разработки и все файлы интерпретатора. после этого проект готов.

Примечание: одна из сторонних зависимостей - PyQT!

## Инструкции для ПОЛЬЗОВАТЕЛЕЙ по установке всего проекта

ВНИМАНИЕ! Для запуска среды разработки и выполнения требуется установленный Python версии 3.10 и платформа Microsoft .NET 8 Runtime. На данный момент поддерживается только платформа Windows 10 и выше

Скачать Microsoft .NET 8: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Скачать Python 3.10: https://www.python.org/downloads/release/python-3100/

1 вариант) Скачайте архив проекта и распакуйте в удобную папку,
чтобы открыть редактор кода запустите .ехе файл среды, 
для удобства можно создать ярлык файла на рабочем столе. Ссылка на архив: {[кликабельный текст](https://drive.google.com/file/d/1V2bumfc-QaoxVG9BybQPtluoM5tNxoZE/view?usp=sharing)}

ВНИМАНИЕ! Windows SmartScreen может ругаться на установщик, это нормально, поскольку нет цифровой подписи и программа выдаёт себя за установщик. В этом случае нажмите кнопку "Подробнее" и "Выполнить в любом случае".

2 вариант) Скачайте установщик и откройте его, далее следуйте его инструкциям: {[кликабельный текст](https://drive.google.com/file/d/1lh8R7iGIHMnY1Lw6y5xon4eksdBKOBQD/view?usp=sharing)}

## Инструкция по сборке интерпретатора для РАЗРАБОТЧИКОВ

ВНИМАНИЕ! Для сборки интерпретатора требуется установленная платформа Microsoft .NET 8 SDK. На данный момент поддерживается только платформа Windows 10 и выше

Скачать: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

1) Клонируйте репозиторий
2) Откройте IDE, поддерживающую C# или воспользуйтесь консолью с установленным .NET 8 SDK.
3) Собирите проект.

Примечание: сторонних зависимостей интерпретатор не имеет!
