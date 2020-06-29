using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterFaceLib;

namespace CncModel
{
    public  class JDInfo:CncInfo
    {
        public override CncMachine GetCncMachine()
        {
            JDCnc jDCNC = new JDCnc();
            jDCNC.cncInfo = this;
            return jDCNC;
        }
    }
}
