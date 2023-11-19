//
// inflate.cpp
//
// $Id: //poco/1.4/Foundation/samples/inflate/src/inflate.cpp#1 $
//
// This sample demonstrates the InflatingInputStream and StreamCopier classes.
//
// Copyright (c) 2004-2006, Applied Informatics Software Engineering GmbH.
// and Contributors.
//
// Permission is hereby granted, free of charge, to any person or organization
// obtaining a copy of the software and accompanying documentation covered by
// this license (the "Software") to use, reproduce, display, distribute,
// execute, and transmit the Software, and to prepare derivative works of the
// Software, and to permit third-parties to whom the Software is furnished to
// do so, all subject to the following:
// 
// The copyright notices in the Software and this entire statement, including
// the above license grant, this restriction and the following disclaimer,
// must be included in all copies of the Software, in whole or in part, and
// all derivative works of the Software, unless such copies or derivative
// works are solely in the form of machine-executable object code generated by
// a source language processor.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
// SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
// FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//


#include "Poco/InflatingStream.h"
#include "Poco/StreamCopier.h"
#include <iostream>
#include <fstream>


using Poco::InflatingInputStream;
using Poco::StreamCopier;


int main(int argc, char** argv)
{
	if (argc != 3)
	{
		std::cout << "usage: " << argv[0] << ": <input_file> <output_file>" << std::endl
		          << "       read deflated (compressed) <input_file>, inflate it and write the result to <output_file>" << std::endl;
		return 1;
	}
	
	std::ifstream istr(argv[1], std::ios::binary);
	if (!istr)
	{
		std::cerr << "cannot open input file: " << argv[1] << std::endl;
		return 2;
	}
	
	std::ofstream ostr(argv[2], std::ios::binary);
	if (!ostr)
	{
		std::cerr << "cannot open output file: " << argv[2] << std::endl;
		return 3;
	}
	
	InflatingInputStream inflater(istr);
	StreamCopier::copyStream(inflater, ostr);
	
	if (!ostr)
	{
		std::cerr << "error writing output file: " << argv[2] << std::endl;
		return 4;
	}
	
	return 0;
}
