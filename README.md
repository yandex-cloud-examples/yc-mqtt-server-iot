# Пример работы с mqtt сервером с использованием библиотеки paho

[Сертификат удостоверяющего центра](https://storage.yandexcloud.net/mqtt/rootCA.crt) включен в пример в виде файла `rootCA.crt` на диске.

Для работы примера нужно создать [реестр](https://cloud.yandex.ru/docs/iot-core/quickstart#create-registry) и [устройство](https://cloud.yandex.ru/docs/iot-core/quickstart#create-device).

Пример фактически делают эхо, то есть посланное в `$devices/<ID_устройства>/events` возвращается клиенту посредством подписки на этот топик от имени реестра и выводится на консоль.

Поддерживаются два способа [авторизации](https://cloud.yandex.ru/docs/iot-core/concepts/authorization), сертификаты или логин и пароль.

## Сертификаты

В примере используются два [сертификата](https://cloud.yandex.ru/docs/iot-core/quickstart#create-ca) - один для устройства, один для реестра.

Чтобы из пары PEM файлов (сертификат и ключ) получить PKCS#12 сертификат в формате pfx, нужно воспользоваться утилитой [openssl](https://www.openssl.org/):

    ```bash
    openssl pkcs12 -export -out cert.pfx -inkey key.pem -in cert.pem
    ```

Расположение на диске:

    ```text
    certs structure:
      /my_registry        Registry directory |currentDir|. Run samples from here.
      `- reg.pfx
      `- dev.pfx
    ```

Пример ищет сертификаты относительно текущей рабочей директории, **поэтому запускать их нужно в папке с сертификатами** (`my_registry` на схеме).

## Логин и пароль

Нужно сгенерировать пароль для [реестра](https://cloud.yandex.ru/docs/iot-core/operations/password/registry-password)
и для [устройства](https://cloud.yandex.ru/docs/iot-core/operations/password/device-password).

Логины реестра и устройства это их `ID`.
