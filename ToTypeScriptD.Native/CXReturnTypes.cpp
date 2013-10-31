// Class1.cpp
#include "pch.h"
#include <Objbase.h>
#include "CXReturnTypes.h"

using namespace ToTypeScriptD::Native;

CXReturnTypes::CXReturnTypes()
{
}

String^ CXReturnTypes::GetString()
{
	return "Hello World!";
}

int32 CXReturnTypes::GetInt32()
{
	return 10;
}

uint32 CXReturnTypes::GetUInt32()
{
	return 10;
}

Guid CXReturnTypes::GetGuid()
{
	GUID result = { 0x4d36e96e, 0xe325, 0x11c3, { 0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18 } };
	Guid gd(result);
	return gd;
}

byte CXReturnTypes::GetByte()
{
	return 10;
}

Platform::Array<byte>^ CXReturnTypes::GetBytes()
{
	unsigned char pBuffer[]="Hello GetBytes Array!";
	Platform::Array<unsigned char>^ arr = ref new Platform::Array<unsigned char>(pBuffer, sizeof(pBuffer));
	return arr;
}

Platform::Boolean CXReturnTypes::GetBoolean()
{
	return true;
}

int CXReturnTypes::MethodWithSomeOutParameters(int in1, Platform::String^ in2, int* out1, Platform::String^* out2, Platform::Array<int>^* out3)
{
	auto temp = ref new Array<int>(10);
	for(unsigned int i = 0; i < temp->Length; i++)
	{
		temp[i] = i;
	}

	*out2 = in2;
	*out1 = in1;
	*out3 = temp;

	return 10;
}

void CXReturnTypes::MethodWithSomeOutParametersButVoidResult(int in1, Platform::String^ in2, int* out1, Platform::String^* out2, Platform::Array<int>^* out3)
{
	auto temp = ref new Array<int>(10);
	for(unsigned int i = 0; i < temp->Length; i++)
	{
		temp[i] = i;
	}

	*out2 = in2;
	*out1 = in1;
	*out3 = temp;
}




Platform::String^ CXReturnTypes::OverloadedMethodWithNumberTypeParams(int64 value)
{
	return "Method with int64/long overload.";
}
Platform::String^ CXReturnTypes::OverloadedMethodWithNumberTypeParams(uint64 value)
{
	return "Method with uint64/ulong overload.";
}
Platform::String^ CXReturnTypes::OverloadedMethodWithNumberTypeParams(double value)
{
	return "Method with double overload.";
}
