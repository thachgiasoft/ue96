using System; 
using System.Text; 
using System.IO; 
using System.Net; 
using System.Net.Sockets; 
using System.Collections; 
using System.Configuration;
using System.Collections.Specialized;

namespace Icson.Utils
{

	/*
	 <configSections>
	 <!-- ����email������㶨�� -->
	 <section name="mailSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"/>
	 </configSections>
	 <mailSettings>
	 <!--
		���÷���email����Ҫ�ĸ��ֲ���
	 -->
		<add key="Charset" value="gb2312" />
		<add key="From" value="service@baby1one.com.cn.cn" />
		<add key="ReplyTo" value="service@baby1one.com.cn.cn" />
		<add key="MailServer" value="mail.ozzo.com" />
		<add key="MailServerUserName" value="Icson" />
		<add key="MailServerPassWord" value="tom@1998.com" />
	 </mailSettings>
	*/
	/// <summary>
	/// <configSections>
	/// <!-- ����email������㶨�� -->
	/// <section name="mailSettings" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"/>
	/// </configSections>
	/// <mailSettings>
	/// <!--
	/// 	���÷���email����Ҫ�ĸ��ֲ���
	/// -->
	/// 	<add key="Charset" value="gb2312" />
	/// 	<add key="From" value="service@baby1one.com.cn.cn" />
	/// 	<add key="ReplyTo" value="service@baby1one.com.cn.cn" />
	/// 	<add key="MailServer" value="mail.ozzo.com" />
	/// 	<add key="MailServerUserName" value="Icson" />
	/// 	<add key="MailServerPassWord" value="tom@1998.com" />
	/// </mailSettings>
	/// </summary>
	public class TCPMail
	{ 
		#region Private
		private string enter="\r\n"; 
		/// <summary> 
		/// �ռ����б� 
		/// </summary> 
		private Hashtable Recipient=new Hashtable(); 
		/// <summary> 
		/// �ʼ����������� 
		/// </summary>    
		private string mailserver="";
		/// <summary> 
		/// �ʼ��������˿ں� 
		/// </summary>    
		private int mailserverport=25; 
		/// <summary> 
		/// SMTP��֤ʱʹ�õ��û��� 
		/// </summary> 
		private string username=""; 
		/// <summary> 
		/// SMTP��֤ʱʹ�õ����� 
		/// </summary> 
		private string password=""; 
		/// <summary> 
		/// �Ƿ���ҪSMTP��֤ 
		/// </summary>       
		private bool SmtpVerify=true; 
		/// <summary> 
		/// �ʼ������б� 
		/// </summary> 
		private System.Collections.ArrayList Attachments; 
		/// <summary> 
		/// �ʼ��������ȼ���������Ϊ"High","Normal","Low"��"1","3","5" 
		/// </summary> 
		private string priority="Normal"; 		
		/// <summary> 
		/// �ռ������� 
		/// </summary> 
		private int RecipientNum=0; 
		/// <summary> 
		/// ����ռ������� 
		/// </summary> 
		private int recipientmaxnum=10;//cy 
		/// <summary> 
		/// �ܼ��ռ������� 
		/// </summary> 
		//private int RecipientBCCNum=0; 
		/// <summary> 
		/// ������Ϣ���� 
		/// </summary> 
		private string errmsg; 
		/// <summary> 
		/// TcpClient�����������ӷ����� 
		/// </summary>    
		private TcpClient tc; 
		/// <summary> 
		/// NetworkStream���� 
		/// </summary>    
		private NetworkStream ns; 
		/// <summary> 
		/// SMTP��������ϣ�� 
		/// </summary> 
		private Hashtable ErrCodeHT = new Hashtable(); 
		/// <summary> 
		/// SMTP��ȷ�����ϣ�� 
		/// </summary> 
		private Hashtable RightCodeHT = new Hashtable(); 
		/// <summary>
		/// �����˵�ַ
		/// </summary>
		private string from = "";
		/// <summary>
		/// �ʼ�����
		/// </summary>
		private string subject = "";
		/// <summary>
		/// �ʼ�����
		/// </summary>
		private string body = "";
		/// <summary>
		/// �ʼ����͸�ʽ
		/// </summary>
		private bool html = true;
		#endregion

		#region Public
		/// <summary> 
		/// �趨���Դ��룬Ĭ���趨ΪGB2312���粻��Ҫ������Ϊ"" 
		/// </summary> 
		private string charset = "GB2312";
		public string Charset
		{
			set
			{
				charset = value;
			}
			get
			{
				return charset;
			}
		}
		/// <summary> 
		/// �����˵�ַ 
		/// </summary> 
		public string From
		{
			set 
			{
				from = value;
			}
			get
			{
				return from;
			}
		} 
		/// <summary> 
		/// ���������� 
		/// </summary> 
		public string FromName=""; 
		/// <summary> 
		/// �ظ��ʼ���ַ 
		/// </summary> 
		//public string ReplyTo=""; 
		/// <summary> 
		/// �Ƿ�Html�ʼ� 
		/// </summary>       
		public bool Html
		{
			set
			{
				html = value;
			}
			get
			{
				return html;
			}
		}
		/// <summary> 
		/// �ʼ����� 
		/// </summary>      
		public string Subject
		{
			set
			{
				subject = value;
			}
			get
			{
				return subject;
			}
		}
		/// <summary> 
		/// �ʼ����� 
		/// </summary>       
		public string Body
		{
			set
			{
				body = value;
			}
			get
			{
				return body;
			}
		}
		/// <summary>
		/// �����ʼ���Application
		/// </summary>
		public string MailLord = "";
		#endregion

		public TCPMail() 
		{ 
			Attachments = new System.Collections.ArrayList();
			// Initialize values
			Init();
		}

		/// <summary>
		/// �������ļ��ж�ȡemail��������
		/// </summary>
		private void Init()
		{
			Charset  = AppConfig.MailCharset;
			From = AppConfig.MailFrom;
			FromName = AppConfig.MailFromName;

			mailserver = AppConfig.MailServer;
			username = AppConfig.MailUserName;
			password = AppConfig.MailUserPassword;
			//MailLord = "<br><font color=white>" + AppConfig.MailLord +"</font>";
		}

		/// <summary> 
		/// �ʼ���������������֤��Ϣ 
		/// ���磺"user:pass@www.server.com:25"��Ҳ��ʡ�Դ�Ҫ��Ϣ����"user:pass@www.server.com"��"www.server.com" 
		/// </summary> 		   
		public string MailDomain 
		{ 
			set 
			{ 
				string maildomain=value.Trim(); 
				int tempint; 

				if(maildomain!="") 
				{ 
					tempint=maildomain.IndexOf("@"); 
					if(tempint!=-1) 
					{ 
						string str=maildomain.Substring(0,tempint); 
						MailServerUserName=str.Substring(0,str.IndexOf(":")); 
						MailServerPassWord=str.Substring(str.IndexOf(":")+1,str.Length-str.IndexOf(":")-1); 
						maildomain=maildomain.Substring(tempint+1,maildomain.Length-tempint-1); 
					} 

					tempint=maildomain.IndexOf(":"); 
					if(tempint!=-1) 
					{ 
						mailserver=maildomain.Substring(0,tempint); 
						mailserverport=System.Convert.ToInt32(maildomain.Substring(tempint+1,maildomain.Length-tempint-1)); 
					} 
					else 
					{ 
						mailserver=maildomain; 
					}              
				} 
			} 
		} 

		/// <summary> 
		/// �ʼ��������˿ں� 
		/// </summary>    
		public int MailDomainPort 
		{ 
			set 
			{ 
				mailserverport=value; 
			} 
		} 

		/// <summary> 
		/// SMTP��֤ʱʹ�õ��û��� 
		/// </summary> 
		public string MailServerUserName 
		{ 
			set 
			{ 
				if(value.Trim()!="") 
				{ 
					username=value.Trim(); 
					SmtpVerify=true; 
				} 
				else 
				{ 
					username = ""; 
					SmtpVerify = false; 
				} 
			} 
		} 

		/// <summary> 
		/// SMTP��֤ʱʹ�õ����� 
		/// </summary> 
		public string MailServerPassWord 
		{ 
			set 
			{ 
				password=value; 
			} 
		}    

		/// <summary> 
		/// �ʼ��������ȼ���������Ϊ"High","Normal","Low"��"1","3","5" 
		/// </summary> 
		public string Priority 
		{ 
			set 
			{ 
				switch(value.ToLower()) 
				{ 
					case "high": 
						priority="High"; 
						break; 

					case "1": 
						priority="High"; 
						break; 

					case "normal": 
						priority="Normal"; 
						break; 

					case "3": 
						priority="Normal"; 
						break; 

					case "low": 
						priority="Low"; 
						break; 

					case "5": 
						priority="Low"; 
						break; 

					default: 
						priority="Normal"; 
						break; 
				} 
			} 
		} 


		/// <summary> 
		/// ������Ϣ���� 
		/// </summary>       
		public string ErrorMessage 
		{ 
			get 
			{ 
				return errmsg; 
			} 
		} 

		/// <summary> 
		/// ������������¼ 
		/// </summary> 
		private string logs=""; 
		public string Logs 
		{ 
			get 
			{ 
				return logs; 
			} 
		} 
  
		/// <summary> 
		/// SMTP��Ӧ�����ϣ�� 
		/// </summary> 
		private void SMTPCodeAdd() 
		{ 
			ErrCodeHT.Add("500","�����ַ����"); 
			ErrCodeHT.Add("501","������ʽ����"); 
			ErrCodeHT.Add("502","�����ʵ��"); 
			ErrCodeHT.Add("503","��������ҪSMTP��֤"); 
			ErrCodeHT.Add("504","�����������ʵ��"); 
			ErrCodeHT.Add("421","����δ�������رմ����ŵ�"); 
			ErrCodeHT.Add("450","Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����æ��"); 
			ErrCodeHT.Add("550","Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����δ�ҵ����򲻿ɷ��ʣ�"); 
			ErrCodeHT.Add("451","����Ҫ��Ĳ�������������г���"); 
			ErrCodeHT.Add("551","�û��Ǳ��أ��볢��<forward-path>"); 
			ErrCodeHT.Add("452","ϵͳ�洢���㣬Ҫ��Ĳ���δִ��"); 
			ErrCodeHT.Add("552","�����Ĵ洢���䣬Ҫ��Ĳ���δִ��"); 
			ErrCodeHT.Add("553","�����������ã�Ҫ��Ĳ���δִ�У����������ʽ����"); 
			ErrCodeHT.Add("432","��Ҫһ������ת��"); 
			ErrCodeHT.Add("534","��֤���ƹ��ڼ�"); 
			ErrCodeHT.Add("538","��ǰ�������֤������Ҫ����"); 
			ErrCodeHT.Add("454","��ʱ��֤ʧ��"); 
			ErrCodeHT.Add("530","��Ҫ��֤"); 

			RightCodeHT.Add("220","�������"); 
			RightCodeHT.Add("250","Ҫ����ʼ��������"); 
			RightCodeHT.Add("251","�û��Ǳ��أ���ת����<forward-path>"); 
			RightCodeHT.Add("354","��ʼ�ʼ����룬��<enter>.<enter>����"); 
			RightCodeHT.Add("221","����رմ����ŵ�"); 
			RightCodeHT.Add("334","��������Ӧ��֤Base64�ַ���"); 
			RightCodeHT.Add("235","��֤�ɹ�"); 
		} 


		/// <summary> 
		/// ���ַ�������ΪBase64�ַ��� 
		/// </summary> 
		/// <param name="estr">Ҫ������ַ���</param> 
		private string Base64Encode(string str) 
		{ 
			byte[] barray; 
			barray=Encoding.Default.GetBytes(str); 
			return Convert.ToBase64String(barray); 
		} 


		/// <summary> 
		/// ��Base64�ַ�������Ϊ��ͨ�ַ��� 
		/// </summary> 
		/// <param name="dstr">Ҫ������ַ���</param> 
		private string Base64Decode(string str) 
		{ 
			byte[] barray; 
			barray=Convert.FromBase64String(str); 
			return Encoding.Default.GetString(barray); 
		} 

       
		/// <summary> 
		/// �õ��ϴ��������ļ��� 
		/// </summary> 
		/// <param name="FilePath">�����ľ���·��</param> 
		private string GetStream(string FilePath) 
		{ 
			//�����ļ������� 
			System.IO.FileStream FileStr=new System.IO.FileStream(FilePath,System.IO.FileMode.Open); 
			byte[] by=new byte[System.Convert.ToInt32(FileStr.Length)]; 
			FileStr.Read(by,0,by.Length); 
			FileStr.Close(); 
			return(System.Convert.ToBase64String(by)); 
		} 


		/// <summary> 
		/// ����ʼ����� 
		/// </summary> 
		/// <param name="path">��������·��</param> 
		public void AddAttachment(string path) 
		{ 
			Attachments.Add(path); 
		} 
        
		/// <summary> 
		/// ���һ���ռ��� 
		/// </summary>    
		/// <param name="str">�ռ��˵�ַ</param> 
		public bool AddRecipient(string str) 
		{ 
			str=str.Trim(); 
			if(str==null||str==""||str.IndexOf("@")==-1) 
				return false; //�˴�ԭ��return true���ѽ⣬Kyle
			if(RecipientNum<recipientmaxnum) 
			{ 
				Recipient.Add(RecipientNum,str); 
				RecipientNum++;             
				return true; 
			} 
			else 
			{ 
				errmsg+="�ռ��˹���"; 
				return false; 
			} 
		} 


		/// <summary> 
		/// ����ռ������� 
		/// </summary> 
		public int RecipientMaxNum 
		{ 
			set 
			{ 
				recipientmaxnum = value; 
			} 
		} 

		/// <summary> 
		/// ���һ���ռ��ˣ�������recipientmaxnum����������Ϊ�ַ������� 
		/// </summary> 
		/// <param name="str">�������ռ��˵�ַ���ַ������飨������recipientmaxnum����</param>    
		public bool AddRecipient(string[] str) 
		{ 
			for(int i=0;i<str.Length;i++) 
			{ 
				if(!AddRecipient(str[i])) 
				{ 
					return false; 
				} 
			} 
			return true; 
		} 

		/// <summary> 
		/// ����SMTP����
		/// </summary>    
		private bool SendCommand(string str) 
		{ 
			byte[]  WriteBuffer; 
			if(str==null||str.Trim()=="") 
			{ 
				return true; 
			} 
			logs+=str; 
			WriteBuffer = Encoding.Default.GetBytes(str); 
			try 
			{ 
				ns.Write(WriteBuffer,0,WriteBuffer.Length); 
			} 
			catch 
			{ 
				errmsg="�������Ӵ���"; 
				return false; 
			} 
			return true; 
		} 

		/// <summary> 
		/// ����SMTP��������Ӧ
		/// </summary> 
		private string RecvResponse() 
		{ 
			int StreamSize; 
			string ReturnValue = ""; 
			byte[]  ReadBuffer = new byte[1024] ; 
			try 
			{ 
				StreamSize=ns.Read(ReadBuffer,0,ReadBuffer.Length); 
			} 
			catch 
			{ 
				errmsg="�������Ӵ���"; 
				return "false"; 
			} 

			if (StreamSize==0) 
			{ 
				return ReturnValue ; 
			} 
			else 
			{ 
				ReturnValue = Encoding.Default.GetString(ReadBuffer).Substring(0,StreamSize); 
				logs+=ReturnValue; 
				return  ReturnValue; 
			} 
		} 


		/// <summary> 
		/// �����������������һ��������ջ�Ӧ�� 
		/// </summary> 
		/// <param name="Command">һ��Ҫ���͵�����</param> 
		/// <param name="errstr">�������Ҫ��������Ϣ</param> 
		private bool Dialog(string str,string errstr) 
		{ 
			if(str==null||str.Trim()=="") 
			{ 
				return true; 
			} 
			if(SendCommand(str)) 
			{ 
				string RR=RecvResponse(); 
				if(RR=="false") 
				{ 
					return false; 
				} 
				string RRCode=RR.Substring(0,3); 
				if(RightCodeHT[RRCode]!=null) 
				{ 
					return true; 
				} 
				else 
				{ 
					if(ErrCodeHT[RRCode]!=null) 
					{ 
						errmsg+=(RRCode+ErrCodeHT[RRCode].ToString()); 
						errmsg+=enter; 
					} 
					else 
					{ 
						errmsg+=RR; 
					} 
					errmsg+=errstr; 
					return false; 
				} 
			} 
			else 
			{ 
				return false; 
			} 

		} 


		/// <summary> 
		/// �����������������һ��������ջ�Ӧ�� 
		/// </summary> 

		private bool Dialog(string[] str,string errstr) 
		{ 
			for(int i=0;i<str.Length;i++) 
			{ 
				if(!Dialog(str[i],"")) 
				{ 
					errmsg+=enter; 
					errmsg+=errstr; 
					return false; 
				} 
			} 

			return true; 
		} 
		private bool sendEmail() 
		{ 
			//�������� 
			try 
			{ 
				tc=new TcpClient(mailserver,mailserverport); 
			} 
			catch(Exception e) 
			{ 
				errmsg=e.ToString(); 
				return false; 
			} 

			ns = tc.GetStream(); 
			SMTPCodeAdd(); 

			//��֤���������Ƿ���ȷ 
			if(RightCodeHT[RecvResponse().Substring(0,3)]==null) 
			{ 
				errmsg="��������ʧ��"; 
				return false; 
			} 


			string[] SendBuffer; 
			string SendBufferstr; 

			//����SMTP��֤ 
			if(SmtpVerify) 
			{ 
				SendBuffer=new String[4]; 
				SendBuffer[0]="EHLO " + mailserver + enter; 
				SendBuffer[1]="AUTH LOGIN" + enter; 
				SendBuffer[2]=Base64Encode(username) + enter; 
				SendBuffer[3]=Base64Encode(password) + enter; 
				if(!Dialog(SendBuffer,"SMTP��������֤ʧ�ܣ���˶��û��������롣")) 
					return false; 
			} 
			else 
			{ 
				SendBufferstr="HELO " + mailserver + enter; 
				if(!Dialog(SendBufferstr,"")) 
					return false; 
			} 

			// 
			SendBufferstr="MAIL FROM:<" + From + ">" + enter; 
			if(!Dialog(SendBufferstr,"�����˵�ַ���󣬻���Ϊ��")) 
				return false; 

			// 
			SendBuffer=new string[recipientmaxnum]; 
			for(int i=0;i<Recipient.Count;i++) 
			{ 

				SendBuffer[i]="RCPT TO:<" + Recipient[i].ToString() +">" + enter; 

			} 
			if(!Dialog(SendBuffer,"�ռ��˵�ַ����")) 
				return false; 

			SendBufferstr="DATA" + enter; 
			if(!Dialog(SendBufferstr,"")) 
				return false; 

			SendBufferstr="From:" + FromName + "<" + From +">" +enter; 

			/* SendBufferstr += "To:=?"+Charset.ToUpper()+"?B?"+Base64Encode(RecipientName)+"?="+"<"+Recipient[0]+">"+enter; 
			SendBufferstr+="CC:"; 
			for(int i=0;i<Recipient.Count;i++) 
			{ 
				SendBufferstr+=Recipient[i].ToString() + "<" + Recipient[i].ToString() +">,"; 
			} 
			SendBufferstr+=enter; */
			//==========Modified By Rickie================
			// Jul. 31, 2003
			SendBufferstr += "To:";
			for(int i=0;i<Recipient.Count;i++) 
			{ 
				SendBufferstr+=Recipient[i].ToString() + "<" + Recipient[i].ToString() +">,"; 
			} 
			SendBufferstr+=enter;
			//SendBufferstr+="CC:"; 
			//SendBufferstr+=enter;
			// ========= END ===========
			if(Charset=="") 
			{ 
				SendBufferstr+="Subject:" + Subject + enter; 
			} 
			else 
			{ 
				SendBufferstr+="Subject:" + "=?" + Charset.ToUpper() + "?B?" + Base64Encode(Subject) +"?=" +enter; 
			} 

			SendBufferstr+="X-Priority:" + priority + enter; 
			SendBufferstr+="X-MSMail-Priority:" + priority + enter; 
			SendBufferstr+="Importance:" + priority + enter; 
			SendBufferstr+="X-Mailer: Huolx.Pubclass" + enter; 
			SendBufferstr+="MIME-Version: 1.0" + enter; 

			SendBufferstr += "Content-Type: multipart/mixed;"+enter;//���ݸ�ʽ�ͷָ��� 
			SendBufferstr += "   boundary=\"----=_NextPart_000_00D6_01C29593.AAB31770\""+enter; 
			SendBufferstr += "------=_NextPart_000_00D6_01C29593.AAB31770"+enter; 

			if(Html) 
			{ 
				SendBufferstr+="Content-Type: text/html;" + enter; 
			} 
			else 
			{ 
				SendBufferstr+="Content-Type: text/plain;" + enter; 
			} 

			if(Charset=="") 
			{ 
				SendBufferstr+="   charset=\"iso-8859-1\"" + enter; 
			} 
			else 
			{ 
				SendBufferstr+="   charset=\"" + Charset.ToLower() + "\"" + enter; 
			} 
			//SendBufferstr += "Content-Transfer-Encoding: base64"+enter; 

			SendBufferstr+="Content-Transfer-Encoding: base64" + enter + enter; 

			SendBufferstr+= Base64Encode(Body+MailLord) + enter; 
			// ��Ҫ֪������һ��������������
			//SendBufferstr+= Base64Encode(MailLord) + enter; 

			if(Attachments.Count!=0) 
			{ 
				foreach(string filepath in Attachments) 
				{ 
					SendBufferstr += "------=_NextPart_000_00D6_01C29593.AAB31770"+enter; 
					SendBufferstr += "Content-Type: application/octet-stream"+enter; 
					SendBufferstr += "   name=\"=?"+Charset.ToUpper()+"?B?"+Base64Encode(filepath.Substring(filepath.LastIndexOf("\\")+1))+"?=\""+enter; 
					SendBufferstr += "Content-Transfer-Encoding: base64"+enter; 
					SendBufferstr += "Content-Disposition: attachment;"+enter; 
					SendBufferstr += "   filename=\"=?"+Charset.ToUpper()+"?B?"+Base64Encode(filepath.Substring(filepath.LastIndexOf("\\")+1))+"?=\""+enter+enter; 
					SendBufferstr += GetStream(filepath)+enter+enter; 
				} 
			} 
			SendBufferstr += "------=_NextPart_000_00D6_01C29593.AAB31770--"+enter+enter; 
          
          
			SendBufferstr += enter + "." + enter; 
		
			if(!Dialog(SendBufferstr,"�����ż���Ϣ")) 
				return false; 


			SendBufferstr="QUIT" + enter; 
			if(!Dialog(SendBufferstr,"�Ͽ�����ʱ����")) 
				return false; 

			ns.Close(); 
			tc.Close(); 
			return true; 
		} 

		public bool Send(string address, string subject, string body)
		{
			if ( !AppConfig.IsSendEMail)
				return false;
			AddRecipient(address);
			Subject = subject;
			Body = body;
			return sendEmail();
		}

	} 
}
