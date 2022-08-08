using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KeyAuth
{
	// Token: 0x02000005 RID: 5
	public class json_wrapper
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00003937 File Offset: 0x00001B37
		public static bool is_serializable(Type to_check)
		{
			return to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003954 File Offset: 0x00001B54
		public json_wrapper(object obj_to_work_with)
		{
			this.current_object = obj_to_work_with;
			Type type = this.current_object.GetType();
			this.serializer = new DataContractJsonSerializer(type);
			if (!json_wrapper.is_serializable(type))
			{
				throw new Exception(string.Format("the object {0} isn't a serializable", this.current_object));
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000039A4 File Offset: 0x00001BA4
		public object string_to_object(string json)
		{
			object result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(json)))
			{
				result = this.serializer.ReadObject(memoryStream);
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000039EC File Offset: 0x00001BEC
		public T string_to_generic<T>(string json)
		{
			return (T)((object)this.string_to_object(json));
		}

		// Token: 0x0400000C RID: 12
		private DataContractJsonSerializer serializer;

		// Token: 0x0400000D RID: 13
		private object current_object;
	}
}
