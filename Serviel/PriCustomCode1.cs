using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomCode;
using Primavera.Extensibility.Extensions;
using StdPlatBS100;

namespace NightAudit
{
    public class PriCustomCode1 : CustomCode

    {
        public void openform()
        {
            StdBSTipos.ResultMsg resultadoPergunta = new StdBSTipos.ResultMsg();
            resultadoPergunta = PSO.Dialogos.MostraMensagem(StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimNao, "Pretende aceder ao módulo de night Audit?", StdPlatBS100.StdBSTipos.IconId.PRI_Questiona);
            if (resultadoPergunta == StdBSTipos.ResultMsg.PRI_Sim)
            {
                try
                {
                    using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(frm_NightAudit)))
                    {
                        if (result.IsSuccess())
                        {
                            frm_NightAudit.Plataforma = PSO;
                            frm_NightAudit.MotorLE = BSO;
                            string conString = PSO.BaseDados.DaConnectionStringNET(PSO.BaseDados.DaNomeBDdaEmpresa(BSO.Contexto.CodEmp), "DEFAULT");
                            frm_NightAudit.constString = conString;
                            (result.Result as frm_NightAudit).ShowDialog();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
