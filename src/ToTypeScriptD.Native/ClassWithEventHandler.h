#pragma once
#include "SampleEnum.h"

namespace ToTypeScriptD
{
	namespace Native
	{

		ref class ClassWithEventHandler;
		public delegate void SomethingHappenedEventHandler(ClassWithEventHandler^ sender, Platform::String^ s, int foo);
		
		public ref class ClassWithEventHandler sealed
		{
		public:
			ClassWithEventHandler();
			event SomethingHappenedEventHandler^ SomethingHappened;
			void DoSomething();
			void DoSomethingTyped();

		internal:

			event Windows::Foundation::TypedEventHandler<ClassWithEventHandler^, ToTypeScriptD::Native::SampleEnum>^ SampleTyped;

		};

	}
}
