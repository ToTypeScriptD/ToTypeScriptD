using namespace Platform;

namespace ToTypeScriptD
{
	namespace Native{
		public ref class CXReturnTypes sealed
		{
		public:
			CXReturnTypes();
			Platform::String^ GetString();
			int32 GetInt32();
			uint32 GetUInt32();
			Platform::Guid GetGuid();
			byte GetByte();
			Platform::Array<byte>^ GetBytes();
			Platform::Boolean GetBoolean();

			int MethodWithSomeOutParameters(int in1, Platform::String^ in2, int* out1, Platform::String^* out2, Platform::Array<int>^* out3);
		};
	}
}