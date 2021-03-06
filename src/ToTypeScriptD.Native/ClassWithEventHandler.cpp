#include "pch.h"
#include "ClassWithEventHandler.h"

using namespace ToTypeScriptD::Native;

ClassWithEventHandler::ClassWithEventHandler(void)
{
}

void ClassWithEventHandler::DoSomething()
{
	//Do something....

	// ...then fire the event:
	SomethingHappened(this, L"Something happened.", 10);
}


void ClassWithEventHandler::DoSomethingTyped()
{
	//Do something....

	// ...then fire the event:
	SampleTyped(this, SampleEnum::B);
}