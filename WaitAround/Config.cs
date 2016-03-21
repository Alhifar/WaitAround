using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WaitAround
{
    public class Config
    {
        public String menuKey { get; set; }

        public Config(WaitAround mod)
        {
            var configLocation = Path.Combine(mod.PathOnDisk, "Config.json");
            if (!File.Exists(configLocation))
            {
                Console.WriteLine("The config file for WaitAround was not found, attempting creation...");

                this.menuKey = "K";

                File.WriteAllBytes(configLocation, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this)));
            }
            else
            {
                Dictionary<string, string> c = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.UTF8.GetString(File.ReadAllBytes(configLocation)));

                this.menuKey = c["menuKey"];
            }
            Console.WriteLine("The config file for WaitAround has been loaded.\n\tmenuKey: {0}", this.menuKey);
        }
    }
}
