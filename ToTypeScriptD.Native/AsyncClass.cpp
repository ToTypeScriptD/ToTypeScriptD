// AsyncClass.cpp
#include "pch.h"
#include <ppltasks.h>

#include "AsyncClass.h"

using namespace concurrency;

using namespace ToTypeScriptD::Native;

AsyncClass::AsyncClass()
{
}

IAsyncOperation<String^>^ AsyncClass::GetStringAsync(String^ value)
{
	return create_async([value] () {
		return value;
	});
}