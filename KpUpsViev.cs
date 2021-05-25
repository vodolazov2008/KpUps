using Scada.Data.Configuration;
using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    public sealed class KpUpsViev : KPView
    {
        public override string KPDescr
        {
            get
            {
                return "Библиотека КП для тестирования.";
            }
        }

        public override KPCnlPrototypes DefaultCnls
        {
            get
            {
                KPCnlPrototypes kPCnlPrototypes = new KPCnlPrototypes();
                List<InCnlPrototype> inCnls = kPCnlPrototypes.InCnls;
                inCnls.Add(new InCnlPrototype("Status", BaseValues.CnlTypes.TI)
                {
                    Signal = 0,
                    ParamName = "Status",
                    ShowNumber = false,
                    EvEnabled = true,
                    EvOnChange = true
                }) ;
                inCnls.Add(new InCnlPrototype("InputLineVoltage", BaseValues.CnlTypes.TI)
                {
                    Signal = 1,
                    ParamName = "InputLineVoltage",
                    ShowNumber = true,
                    UnitName = "V",
                    EvEnabled = false,
                    EvOnChange = false
                });
                inCnls.Add(new InCnlPrototype("BatteryActualVoltage", BaseValues.CnlTypes.TI)
                {
                    Signal = 2,
                    ParamName = "BatteryActualVoltage",
                    ShowNumber = true,
                    UnitName = "V",
                    EvEnabled = false,
                    EvOnChange = false
                });
                inCnls.Add(new InCnlPrototype("BatteryCapacity", BaseValues.CnlTypes.TI)
                {
                    Signal = 3,
                    ParamName = "BatteryCapacity",
                    ShowNumber = true,
                    DecDigits = 0,
                    UnitName = "%",
                    EvEnabled = false,
                    EvOnChange = false
                });
                inCnls.Add(new InCnlPrototype("BatteryRunTimeRemaining", BaseValues.CnlTypes.TI)
                {
                    Signal = 4,
                    ParamName = "BatteryRunTimeRemaining",
                    ShowNumber = false,
                    EvEnabled = false,
                    EvOnChange = false
                });
                inCnls.Add(new InCnlPrototype("OutputLoad", BaseValues.CnlTypes.TI)
                {
                    Signal = 5,
                    ParamName = "OutputLoad",
                    ShowNumber = true,
                    DecDigits = 0,
                    UnitName = "%",
                    EvEnabled = false,
                    EvOnChange = false
                });
                return kPCnlPrototypes;
            }
            
        }

    }
}