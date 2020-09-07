using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CurrenciesGrabber.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				System.Console.WriteLine("Начинаем работу.");
				var api = new ApiService();
                System.Console.WriteLine("Получаем курсы валют.");
                var allRates = api.GetAllRates();
				System.Console.WriteLine($"Получено [{allRates.Count}] курсов.");

				System.Console.WriteLine("Записываем данные.");

				System.Console.WriteLine("Данные записаны, нажмите Enter для выхода.");
			}
			catch(Exception e)
			{
				System.Console.WriteLine($"Произошла ошибка. [{e}]");
				System.Console.WriteLine("Нажмите Enter для выхода.");
			}
			System.Console.ReadLine();
		}
	}

	public class ApiService
	{
		private MyWebClient client;

		public ApiService()
		{
			client = new MyWebClient();
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate{ return true; });
		}

		public List<Currency> GetAllCurrencies()
		{
			var json = client.DownloadString("http://www.nbrb.by/api/exrates/currencies");

			return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Currency>>(json);
		}

        public List<Rate> GetAllRates()
		{
			var json = client.DownloadString("https://www.nbrb.by/api/exrates/rates?periodicity=0");

			return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Rate>>(json);
		}
	}

class MyWebClient : WebClient
	{
		protected override WebRequest GetWebRequest(Uri address)
		{
			HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
			request.ClientCertificates.Add(new X509Certificate());
			return request;
		}
	}



	public class Currency
	{
		public int Cur_ID { get; set; }
		public Nullable<int> Cur_ParentID { get; set; }
		public string Cur_Code { get; set; }
		public string Cur_Abbreviation { get; set; }
		public string Cur_Name { get; set; }
		public string Cur_Name_Bel { get; set; }
		public string Cur_Name_Eng { get; set; }
		public string Cur_QuotName { get; set; }
		public string Cur_QuotName_Bel { get; set; }
		public string Cur_QuotName_Eng { get; set; }
		public string Cur_NameMulti { get; set; }
		public string Cur_Name_BelMulti { get; set; }
		public string Cur_Name_EngMulti { get; set; }
		public int Cur_Scale { get; set; }
		public int Cur_Periodicity { get; set; }
		public System.DateTime Cur_DateStart { get; set; }
		public System.DateTime Cur_DateEnd { get; set; }
	}

	public class Rate
	{
		public int Cur_ID { get; set; }
		public DateTime Date { get; set; }
		public string Cur_Abbreviation { get; set; }
		public int Cur_Scale { get; set; }
		public string Cur_Name { get; set; }
		public decimal? Cur_OfficialRate { get; set; }
	}

	public class RateShort
	{
		public int Cur_ID { get; set; }
		public System.DateTime Date { get; set; }
		public decimal? Cur_OfficialRate { get; set; }
	}
}
