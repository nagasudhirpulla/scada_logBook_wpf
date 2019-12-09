using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftLogDisplayApp
{
    public class ConfigurationManager
    {
        public string FreqPnt { get; set; } = "WRLDCMP.SCADA1.A0036324";
        public string DemPnt { get; set; } = "WRLDCMP.SCADA1.A0047000";
        public string WrErPnt { get; set; } = "WRLDCMP.SCADA1.A0043398";
        public string WrSrPnt { get; set; } = "WRLDCMP.SCADA1.A0043402";
        public string WrNrPnt { get; set; } = "WRLDCMP.SCADA1.A0043400";
        public string WrIrPnt { get; set; } = "WRLDCMP.SCADA1.A0043311";
        public string configFilename = "logBookConfig.json";

        public string GetAppDataPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public void Initialize()
        {
            string path = GetAppDataPath();
            string jsonPath = path + "\\" + configFilename;

            if (File.Exists(jsonPath))
            {
                ConfigurationManager configObj = JsonConvert.DeserializeObject<ConfigurationManager>(File.ReadAllText(jsonPath));
                CloneFromObject(configObj);
            }
            else
            {
                Save();
            }
        }

        public void Save()
        {
            string ConfigJSONStr = JsonConvert.SerializeObject(this, Formatting.Indented);

            string path = GetAppDataPath();
            string jsonPath = path + "\\" + configFilename;

            File.WriteAllText(jsonPath, ConfigJSONStr);
        }

        public void CloneFromObject(ConfigurationManager configuration)
        {
            FreqPnt = configuration.FreqPnt;
            DemPnt = configuration.DemPnt;
            WrErPnt = configuration.WrErPnt;
            WrSrPnt = configuration.WrSrPnt;
            WrNrPnt = configuration.WrNrPnt;
            WrIrPnt = configuration.WrIrPnt;
        }
    }
}
