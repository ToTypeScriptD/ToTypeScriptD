#pragma once

namespace ToTypeScriptD
{
	namespace Native
	{

		ref class ClassWithEventHandler;
		public delegate void SomethingHappenedEventHandler(ClassWithEventHandler^ sender, Platform::String^ s);

		public ref class ClassWithEventHandler sealed
		{
		public:
			ClassWithEventHandler();
			event SomethingHappenedEventHandler^ SomethingHappened;
			void DoSomething();

		};
	}
}
