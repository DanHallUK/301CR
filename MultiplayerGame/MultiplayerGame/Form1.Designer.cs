namespace MultiplayerGame
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonConnectChatServer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.clientName = new System.Windows.Forms.TextBox();
            this.chatText = new System.Windows.Forms.TextBox();
            this.messageText = new System.Windows.Forms.TextBox();
            this.sendMessage = new System.Windows.Forms.Button();
            this.serverAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(557, 437);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // buttonConnectChatServer
            // 
            this.buttonConnectChatServer.Location = new System.Drawing.Point(576, 13);
            this.buttonConnectChatServer.Name = "buttonConnectChatServer";
            this.buttonConnectChatServer.Size = new System.Drawing.Size(134, 42);
            this.buttonConnectChatServer.TabIndex = 5;
            this.buttonConnectChatServer.Text = "Connect Server";
            this.buttonConnectChatServer.UseVisualStyleBackColor = true;
            this.buttonConnectChatServer.Click += new System.EventHandler(this.buttonConnectChatServer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(715, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Username:";
            // 
            // clientName
            // 
            this.clientName.Location = new System.Drawing.Point(779, 39);
            this.clientName.Name = "clientName";
            this.clientName.Size = new System.Drawing.Size(100, 20);
            this.clientName.TabIndex = 7;
            this.clientName.Text = "User";
            // 
            // chatText
            // 
            this.chatText.Location = new System.Drawing.Point(576, 65);
            this.chatText.Multiline = true;
            this.chatText.Name = "chatText";
            this.chatText.Size = new System.Drawing.Size(304, 355);
            this.chatText.TabIndex = 8;
            // 
            // messageText
            // 
            this.messageText.Location = new System.Drawing.Point(576, 430);
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(211, 20);
            this.messageText.TabIndex = 9;
            this.messageText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageText_KeyDown);
            // 
            // sendMessage
            // 
            this.sendMessage.Location = new System.Drawing.Point(793, 423);
            this.sendMessage.Name = "sendMessage";
            this.sendMessage.Size = new System.Drawing.Size(87, 32);
            this.sendMessage.TabIndex = 10;
            this.sendMessage.Text = "Send Message";
            this.sendMessage.UseVisualStyleBackColor = true;
            this.sendMessage.Click += new System.EventHandler(this.sendMessage_Click);
            // 
            // serverAddress
            // 
            this.serverAddress.Location = new System.Drawing.Point(779, 13);
            this.serverAddress.Name = "serverAddress";
            this.serverAddress.Size = new System.Drawing.Size(100, 20);
            this.serverAddress.TabIndex = 12;
            this.serverAddress.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(715, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "IP Address:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 462);
            this.Controls.Add(this.serverAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sendMessage);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.chatText);
            this.Controls.Add(this.clientName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonConnectChatServer);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonConnectChatServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox clientName;
        private System.Windows.Forms.TextBox chatText;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.Button sendMessage;
        private System.Windows.Forms.TextBox serverAddress;
        private System.Windows.Forms.Label label1;
    }
}

