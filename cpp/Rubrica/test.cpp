/*
version 1.0

Written by Ryick

(add your name and modify the version number if you update the file)
*/

#include "voce.h"

using namespace std;

int main(void) {
	Voce *v = new Voce("John", "Foobar", "404 424242");
	cout << *v << endl;
	return 0;
}
//eof test.cpp
