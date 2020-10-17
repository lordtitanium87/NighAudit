using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BasBE100;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.CashManagement.Editors;
using Primavera.Extensibility.CustomForm;
using Primavera.Extensibility.Extensions;
using Primavera.Extensibility.Internal.Editors;
using System.ComponentModel;
using StdBE100;
using IntBE100;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using StdPlatBS100;

namespace NightAudit.CashManagement
{
    public class UiEditorTesouraria : EditorTesouraria
    {
        public override void DocumentoIdentificado(BasBETiposGcp.TE_DocTesouraria TDocumento, string Documento, ref bool Cancel, ExtensibilityEventArgs e)
        {
            //StdBSTipos.ResultMsg resultadoPergunta = new StdBSTipos.ResultMsg();

            //if (Documento == "FCHCX")
            //{
            //    resultadoPergunta = PSO.Dialogos.MostraMensagem(StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimNao, "Pretende aceder ao módulo de night Audit?", StdPlatBS100.StdBSTipos.IconId.PRI_Questiona);                
            //    if (resultadoPergunta == StdBSTipos.ResultMsg.PRI_Sim)
            //    {
            //        try
            //        {
            //            using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(frm_NightAudit)))
            //            {
            //                if (result.IsSuccess())
            //                {
            //                    frm_NightAudit.Plataforma = PSO;
            //                    frm_NightAudit.MotorLE = BSO;
            //                    (result.Result as frm_NightAudit).ShowDialog();
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    }
            //}
        }
    }
}