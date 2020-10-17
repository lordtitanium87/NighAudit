namespace NightAudit
{
    partial class frm_NightAudit
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
            this.btnNovo = new System.Windows.Forms.ToolStripMenuItem();
            this.Converter = new System.Windows.Forms.ToolStripMenuItem();
            this.Imprimir = new System.Windows.Forms.ToolStripMenuItem();
            this.Anular = new System.Windows.Forms.ToolStripMenuItem();
            this.Sair = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.stdDatePicker2 = new PRISDK100.StdDatePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.stdDatePicker1 = new PRISDK100.StdDatePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.f4TabelaSQL1 = new PRISDK100.F4TabelaSQL();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblAnulado = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.f4TabelaSQL2 = new PRISDK100.F4TabelaSQL();
            this.label3 = new System.Windows.Forms.Label();
            this.priGrelha1 = new PRISDK100.PriGrelha();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Empresa = new System.Windows.Forms.ToolStripStatusLabel();
            this.User = new System.Windows.Forms.ToolStripStatusLabel();
            this.TopMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopMenu
            // 
            this.TopMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Converter,
            this.Atualizar,
            this.btnNovo,
            this.Anular,
            this.Imprimir,
            this.Sair});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(942, 24);
            this.TopMenu.TabIndex = 40;
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
            // btnNovo
            // 
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(48, 20);
            this.btnNovo.Text = "Novo";
            this.btnNovo.Click += new System.EventHandler(this.btnNovo_Click);
            // 
            // Converter
            // 
            this.Converter.Image = global::NightAudit.Properties.Resources.processar;
            this.Converter.Name = "Converter";
            this.Converter.Size = new System.Drawing.Size(120, 20);
            this.Converter.Text = "Processar Fecho";
            this.Converter.Click += new System.EventHandler(this.Converter_Click);
            // 
            // Imprimir
            // 
            this.Imprimir.Name = "Imprimir";
            this.Imprimir.Size = new System.Drawing.Size(65, 20);
            this.Imprimir.Text = "Imprimir";
            // 
            // Anular
            // 
            this.Anular.Image = global::NightAudit.Properties.Resources.anular;
            this.Anular.Name = "Anular";
            this.Anular.Size = new System.Drawing.Size(70, 20);
            this.Anular.Text = "Anular";
            this.Anular.Click += new System.EventHandler(this.Anular_Click);
            // 
            // Sair
            // 
            this.Sair.Image = global::NightAudit.Properties.Resources.cancelar;
            this.Sair.Name = "Sair";
            this.Sair.Size = new System.Drawing.Size(54, 20);
            this.Sair.Text = "Sair";
            this.Sair.Click += new System.EventHandler(this.Sair_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.stdDatePicker2);
            this.groupBox1.Controls.Add(this.f4TabelaSQL2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.stdDatePicker1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.f4TabelaSQL1);
            this.groupBox1.Location = new System.Drawing.Point(4, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(935, 78);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cabeçalho";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(487, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(83, 21);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(576, 20);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // stdDatePicker2
            // 
            this.stdDatePicker2.CustomFormat = "yyyy-MM-dd";
            this.stdDatePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.stdDatePicker2.Location = new System.Drawing.Point(824, 47);
            this.stdDatePicker2.Name = "stdDatePicker2";
            this.stdDatePicker2.Size = new System.Drawing.Size(101, 20);
            this.stdDatePicker2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(729, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data Documento";
            // 
            // stdDatePicker1
            // 
            this.stdDatePicker1.CustomFormat = "yyyy-MM-dd";
            this.stdDatePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.stdDatePicker1.Location = new System.Drawing.Point(824, 20);
            this.stdDatePicker1.Name = "stdDatePicker1";
            this.stdDatePicker1.Size = new System.Drawing.Size(101, 20);
            this.stdDatePicker1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(729, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data de Fecho";
            // 
            // f4TabelaSQL1
            // 
            this.f4TabelaSQL1.AliasCampoChave = "CDU_Codigo";
            this.f4TabelaSQL1.CampoChave = "CDU_Codigo";
            this.f4TabelaSQL1.CampoDescricao = "CDU_Descricao";
            this.f4TabelaSQL1.Caption = "Documento:    ";
            this.f4TabelaSQL1.F4Modal = false;
            this.f4TabelaSQL1.Inicializado = false;
            this.f4TabelaSQL1.Location = new System.Drawing.Point(9, 20);
            this.f4TabelaSQL1.MaxLengthF4 = 50;
            this.f4TabelaSQL1.Modulo = "";
            this.f4TabelaSQL1.MostraCaption = true;
            this.f4TabelaSQL1.Name = "f4TabelaSQL1";
            this.f4TabelaSQL1.ResourceID = 0;
            this.f4TabelaSQL1.ResourceIDTituloLista = 0;
            this.f4TabelaSQL1.SelectionFormula = "select cdu_codigo,cdu_descricao,cdu_documentotesouraria from TDU_ResumoDiario ";
            this.f4TabelaSQL1.Size = new System.Drawing.Size(472, 21);
            this.f4TabelaSQL1.TabIndex = 0;
            this.f4TabelaSQL1.TituloLista = "Documento Resumo Diario";
            this.f4TabelaSQL1.WidthCaption = 1300;
            this.f4TabelaSQL1.WidthEspacamento = 65;
            this.f4TabelaSQL1.WidthF4 = 1590;
            this.f4TabelaSQL1.Change += new PRISDK100.F4TabelaSQL.ChangeHandler(this.f4TabelaSQL1_Change);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(4, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(270, 88);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Comissões";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // lblAnulado
            // 
            this.lblAnulado.AutoSize = true;
            this.lblAnulado.Font = new System.Drawing.Font("Microsoft Sans Serif", 45F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnulado.ForeColor = System.Drawing.Color.Red;
            this.lblAnulado.Location = new System.Drawing.Point(603, 113);
            this.lblAnulado.Name = "lblAnulado";
            this.lblAnulado.Size = new System.Drawing.Size(326, 69);
            this.lblAnulado.TabIndex = 8;
            this.lblAnulado.Text = "ANULADO";
            this.lblAnulado.Visible = false;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Location = new System.Drawing.Point(117, 24);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(106, 20);
            this.numericUpDown2.TabIndex = 0;
            this.numericUpDown2.ThousandsSeparator = true;
            this.numericUpDown2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown2_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(229, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // f4TabelaSQL2
            // 
            this.f4TabelaSQL2.AliasCampoChave = "Conta";
            this.f4TabelaSQL2.CampoChave = "Conta";
            this.f4TabelaSQL2.CampoDescricao = "DescBanco";
            this.f4TabelaSQL2.Caption = "Conta Destino:";
            this.f4TabelaSQL2.F4Modal = false;
            this.f4TabelaSQL2.Inicializado = false;
            this.f4TabelaSQL2.Location = new System.Drawing.Point(9, 46);
            this.f4TabelaSQL2.MaxLengthF4 = 50;
            this.f4TabelaSQL2.Modulo = "";
            this.f4TabelaSQL2.MostraCaption = true;
            this.f4TabelaSQL2.Name = "f4TabelaSQL2";
            this.f4TabelaSQL2.ResourceID = 0;
            this.f4TabelaSQL2.ResourceIDTituloLista = 0;
            this.f4TabelaSQL2.SelectionFormula = "select conta,DescBanco,NumConta,Banco,NIB,IBAN,SWIFT,NIF from ContasBancarias whe" +
    "re tipoconta=0";
            this.f4TabelaSQL2.Size = new System.Drawing.Size(472, 21);
            this.f4TabelaSQL2.TabIndex = 5;
            this.f4TabelaSQL2.TituloLista = "Contas Bancárias";
            this.f4TabelaSQL2.WidthCaption = 1300;
            this.f4TabelaSQL2.WidthEspacamento = 65;
            this.f4TabelaSQL2.WidthF4 = 1590;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Comissões Apuradas";
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
            this.priGrelha1.Location = new System.Drawing.Point(4, 207);
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
            this.priGrelha1.Size = new System.Drawing.Size(929, 301);
            this.priGrelha1.TabIndex = 43;
            this.priGrelha1.TituloGrelha = "";
            this.priGrelha1.TituloMapa = "";
            this.priGrelha1.TypeNameLinha = "";
            this.priGrelha1.TypeNameLinhas = "";
            this.priGrelha1.FormatacaoAlterada += new PRISDK100.PriGrelha.FormatacaoAlteradaHandler(this.priGrelha1_FormatacaoAlterada);
            this.priGrelha1.MenuContextoSeleccionado += new PRISDK100.PriGrelha.MenuContextoSeleccionadoHandler(this.priGrelha1_MenuContextoSeleccionado);
            this.priGrelha1.DataFill += new PRISDK100.PriGrelha.DataFillHandler(this.priGrelha1_DataFill);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Empresa,
            this.User});
            this.statusStrip1.Location = new System.Drawing.Point(0, 517);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(942, 22);
            this.statusStrip1.TabIndex = 44;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Empresa
            // 
            this.Empresa.Name = "Empresa";
            this.Empresa.Size = new System.Drawing.Size(118, 17);
            this.Empresa.Text = "toolStripStatusLabel1";
            // 
            // User
            // 
            this.User.Name = "User";
            this.User.Size = new System.Drawing.Size(118, 17);
            this.User.Text = "toolStripStatusLabel1";
            // 
            // frm_NightAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAnulado);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.priGrelha1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TopMenu);
            this.Name = "frm_NightAudit";
            this.Size = new System.Drawing.Size(942, 539);
            this.Text = "Fecho Night Audit";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TransformacaoDocumentos_FormClosed);
            this.Load += new System.EventHandler(this.frm_NightAudit_Load);
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.MenuStrip TopMenu;
        internal System.Windows.Forms.ToolStripMenuItem Atualizar;
        internal System.Windows.Forms.ToolStripMenuItem Converter;
        internal System.Windows.Forms.ToolStripMenuItem Sair;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.GroupBox groupBox2;
        private PRISDK100.PriGrelha priGrelha1;
        private PRISDK100.F4TabelaSQL f4TabelaSQL1;
        private PRISDK100.StdDatePicker stdDatePicker1;
        private System.Windows.Forms.Label label1;
        private PRISDK100.StdDatePicker stdDatePicker2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private PRISDK100.F4TabelaSQL f4TabelaSQL2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripMenuItem Imprimir;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label lblAnulado;
        private System.Windows.Forms.ToolStripMenuItem Anular;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Empresa;
        private System.Windows.Forms.ToolStripStatusLabel User;
        private System.Windows.Forms.ToolStripMenuItem btnNovo;
    }
}