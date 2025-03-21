using System;
using System.Threading;

namespace YandexIoTCoreExample
{
  class Program
  {
    private const string DeviceID = "<your_device_ID>";

    private const bool useCerts = true; // change it if login-password authentication is used

    // used for certificate authentication
    private const string RegistryCertFileName = "<your_registry_cert_filename>";
    private const string DeviceCertFileName = "<your_device_cert_filename>";

    // used for login-password authentication
    private const string RegistryID = "<your_registry_ID>";
    private const string RegistryPassword = "<your_registry_password>";
    private const string DevicePassword = "<your_device_password>";

    private static ManualResetEvent oSubscibedData = new ManualResetEvent(false);

    static void Main(string[] args)
    {
      string topic = YaClient.TopicName(DeviceID, EntityType.Device, TopicType.Events);

      using (YaClient regClient = new YaClient(), devClient = new YaClient())
      {
        if (useCerts) {
          regClient.Start(RegistryCertFileName);
          devClient.Start(DeviceCertFileName);
        } else {
          regClient.Start(RegistryID, RegistryPassword);
          devClient.Start(DeviceID, DevicePassword);
        }

        if (!regClient.WaitConnected() || !devClient.WaitConnected())
        {
          return;
        }
        regClient.SubscribedData += DataHandler;
        regClient.Subscribe(topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).Wait();

        devClient.Publish(topic, "test data", MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).Wait();
        Console.WriteLine($"Published data to: {topic}");

        oSubscibedData.WaitOne();
      }
    }

    private static void DataHandler(string topic, byte[] payload)
    {
      var Payload = System.Text.Encoding.UTF8.GetString(payload);
      Console.WriteLine($"Received data: {topic}:\t{Payload}");
      oSubscibedData.Set();
    }
  }
}
