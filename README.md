# LauncherPrestarter
Установщик Java и GravitLauncher для вашего проекта, написанный на .NET

## Совместимость
Установщик требует .NET 4.6.1 и выше, который предустановлен на Windows 10/11.
Установить эту версию можно на Windows 7 и выше. Многие сборки Windows уже содержат в себе .NET Framework подходящей версии.  

> Prestarter_module работает только с GravitLauncher 5.5.0+

## Установка
1. Установите пакет `dotnet-sdk-8.0` по [инструкции](https://learn.microsoft.com/en-us/dotnet/core/install/linux)
2. Склонируйте репозиторий в доступную для модуля директорию, например в корень LaunchServer
3. Перейдите в каталог `Prestarter_module`
4. Запустите сборку командой `./gradlew build`
5. Собранный файл вы найдете в `build\libs`
6. Установите модуль перемещением файла в `modules`
7. После запуска LaunchServer вы найдете конфигурацию сборки в `config/Prestarter/Config.json`
8. Пропишите путь к файлу проекта, если вы клонировали репозиторий не в корень LaunchServer
9. Выполните сборку лаунчера командой `build`
