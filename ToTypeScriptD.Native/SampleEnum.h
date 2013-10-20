
namespace ToTypeScriptD
{
	namespace Native{
		

		public enum class SampleEnum {
			A,
			B,
			C,
			D
		};

		public enum class SampleEnumNumbered {
			A = 1,
			B = 10,
			C = 100,
			D = 99
		};

		public ref class SampleEnumClass sealed
		{
		public:
			SampleEnumClass();
			SampleEnum MethowWithEnumParameter(SampleEnum value);
		};
	}
}