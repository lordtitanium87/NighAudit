namespace NightAudit
{
    partial class TemplateInterno
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.Atualizar = new System.Windows.Forms.ToolStripMenuItem();
            this.Converter = new System.Windows.Forms.ToolStripMenuItem();
            this.Sair = new System.Windows.Forms.ToolStripMenuItem();
            this.priGrelha1 = new PRISDK100.PriGrelha();
            this.TopMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).BeginInit();
            this.SuspendLayout();
            // 
            // TopMenu
            // 
            this.TopMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Atualizar,
            this.Converter,
            this.Sair});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(705, 24);
            this.TopMenu.TabIndex = 41;
            this.TopMenu.Text = "MenuStrip1";
            // 
            // Atualizar
            // 
            this.Atualizar.Image = global::NightAudit.Properties.Resources.Actualizar;
            this.Atualizar.Name = "Atualizar";
            this.Atualizar.Size = new System.Drawing.Size(81, 20);
            this.Atualizar.Text = "Atualizar";
            this.Atualizar.Click += new System.EventHandler(this.Atualizar_Click);
            // 
            // Converter
            // 
            this.Converter.Image = global::NightAudit.Properties.Resources.processar;
            this.Converter.Name = "Converter";
            this.Converter.Size = new System.Drawing.Size(69, 20);
            this.Converter.Text = "Gravar";
            this.Converter.Click += new System.EventHandler(this.Converter_Click);
            // 
            // Sair
            // 
            this.Sair.Image = global::NightAudit.Properties.Resources.cancelar;
            this.Sair.Name = "Sair";
            this.Sair.Size = new System.Drawing.Size(54, 20);
            this.Sair.Text = "Sair";
            this.Sair.Click += new System.EventHandler(this.Sair_Click);
            // 
            // priGrelha1
            // 
            this.priGrelha1.BackColor = System.Drawing.Color.White;
            this.priGrelha1.BandaMenuContexto = "";
            this.priGrelha1.BotaoConfigurarActiveBar = true;
            this.priGrelha1.BotaoProcurarActiveBar = false;
            this.priGrelha1.CaminhoTemplateImpressao = "";
            this.priGrelha1.Cols = null;
            this.priGrelha1.ColsFrozen = -1;
            this.priGrelha1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.priGrelha1.Location = new System.Drawing.Point(0, 27);
            this.priGrelha1.Name = "priGrelha1";
            this.priGrelha1.NumeroMaxRegistosSemPag = 150000;
            this.priGrelha1.NumeroRegistos = 0;
            this.priGrelha1.NumLinhasCabecalho = 1;
            this.priGrelha1.OrientacaoMapa = PRISDK100.clsSDKTypes.OrientacaoImpressao.oiDefault;
            this.priGrelha1.ParentFormModal = false;
            this.priGrelha1.PermiteActiveBar = true;
            this.priGrelha1.PermiteActualizar = true;
            this.priGrelha1.PermiteAgrupamentosUser = true;
            this.priGrelha1.PermiteConfigurarDetalhes = false;
            this.priGrelha1.PermiteContextoVazia = false;
            this.priGrelha1.PermiteDataFill = false;
            this.priGrelha1.PermiteDetalhes = true;
            this.priGrelha1.PermiteEdicao = false;
            this.priGrelha1.PermiteFiltros = true;
            this.priGrelha1.PermiteGrafico = true;
            this.priGrelha1.PermiteGrandeTotal = false;
            this.priGrelha1.PermiteOrdenacao = true;
            this.priGrelha1.PermitePaginacao = false;
            this.priGrelha1.PermiteScrollBars = true;
            this.priGrelha1.PermiteStatusBar = true;
            this.priGrelha1.PermiteVistas = true;
            this.priGrelha1.PosicionaColunaSeguinte = true;
            this.priGrelha1.Size = new System.Drawing.Size(702, 389);
            this.priGrelha1.TabIndex = 44;
            this.priGrelha1.TituloGrelha = "";
            this.priGrelha1.TituloMapa = "";
            this.priGrelha1.TypeNameLinha = "";
            this.priGrelha1.TypeNameLinhas = "";
            // 
            // TemplateInterno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.priGrelha1);
            this.Controls.Add(this.TopMenu);
            this.Name = "TemplateInterno";
            this.Size = new System.Drawing.Size(705, 416);
            this.Text = "Detalhe de Comissões";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TemplateInterno_FormClosed);
            this.Load += new System.EventHandler(this.TemplateInterno_Load);
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.MenuStrip TopMenu;
        internal System.Windows.Forms.ToolStripMenuItem Atualizar;
        internal System.Windows.Forms.ToolStripMenuItem Converter;
        internal System.Windows.Forms.ToolStripMenuItem Sair;
        private PRISDK100.PriGrelha priGrelha1;
    }
}