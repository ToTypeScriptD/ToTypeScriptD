#pragma once

using namespace Platform;
using namespace Windows::Foundation;

namespace ToTypeScriptD
{
	namespace Native
	{
		public ref class AsyncClass sealed
		{
		public:
			AsyncClass();
			IAsyncOperation<String^>^ GetStringAsync(String^ value);
		};
	}
}
