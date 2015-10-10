#include "roman.h"
#include <iostream>
use namespace namespase std;

int main(int argc, int *argv[]){
	
	int s;
	cin.good();
	cin.exceptions(std::istream::failbit);
	try{
		cout << "Enter an integer" << endl;
		cin >> s;
    } catch(...){
		std::cout << "Inserire un numero!!!!!" << std::endl;
		return 0;
	}
	print_roman(s);
	return 0;
}
