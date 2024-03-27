# LauncherPrestarter
Установщик Java и GravitLauncher для вашего проекта, написанный на .NET

## Совместимость
Установщик требует .NET 4.6.1 и выше, который предустановлен на Windows 10/11.
Установить эту версию можно на Windows 7 и выше. Многие сборки Windows уже содержат в себе .NET Framework подходящей версии.  

> Prestarter_module работает только с GravitLauncher 5.5.0+

## Необходимые зависимости

Установите пакет `dotnet-sdk-8.0` по инструкции [для linux](https://learn.microsoft.com/en-us/dotnet/core/install/linux) или [для windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows)

## Установка в обычном режиме
1. Склонируйте репозиторий
2. Перейдите в каталог `Prestarter`
3. Отредактируйте `Configuration.props`
4. Выполните сборку проекта командой `dotnet publish -o pub -c Release`
5. Готовый для распространения exe вы найдете в `pub`
> **Примечание:** в данной конфигурации престартера, необязательно его нахождение рядом с `Launcher.jar` в `updates` лаунчсервера

## Установка в режиме launch4j
1. Склонируйте репозиторий в доступную для модуля директорию, например в корень LaunchServer
2. Перейдите в каталог `Prestarter_module`
3. Запустите сборку командой `./gradlew build`
4. Собранный файл вы найдете в `build\libs`
5. Установите модуль перемещением файла в `modules`
6. После запуска LaunchServer вы найдете конфигурацию сборки в `config/Prestarter/Config.json`
7. Пропишите путь к файлу проекта, если вы клонировали репозиторий не в корень LaunchServer
8. Выполните сборку лаунчера командой `build`
