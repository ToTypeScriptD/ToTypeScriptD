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
            event Windows::Foundation::TypedEventHandler<ClassWithEventHandler^, ToTypeScriptD::Native::SampleEnum>^ SampleTyped;
			event SomethingHappenedEventHandler^ SomethingHappened;
            //event Windows::Foundation::EventHandler<??> PlainOlEventHandler;
			void DoSomething();
			void DoSomethingTyped();

		};
	}
}
