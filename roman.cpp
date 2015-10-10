#include "roman.h"
#include <iostream>
using namespace std;

int main(int argc, int *argv[]){
	
	int s;
	cin.good();
	cin.exceptions(istream::failbit);
	try{
		cout << "Enter an integer" << endl;
		cin >> s;
    } catch(...){
		cout << "Monkeys do not enter an integer..." << std::endl;
		return 0;
	}
	
	print_roman(s);
	return 0;
}

void print_roman(int n){

	if (n < 1 || n > 3999) {
		cout << "Overflow" << endl;
		return;
	}

	int u, d, c, m; // unità; decine; centinaia; migliaia
	
	u = n % 10; n = (n - u)/10;
	d = n % 10; n = (n - d)/10;
	c = n % 10; n = (n - c)/10;
	m = n;
	
	help(m, 'M', '/0', '/0');
	help(c, 'C', 'D', 'M');
	help(d, 'X', 'L', 'C');
	help(u, 'I', 'V', 'X');

	return;
}

void help(int n, char a, char b, char c){
	switch(n){
		case 0: break;
		case 1: cout << a; break;
		case 2: cout << a << a; break;
		case 3: cout << a << a << a; break;
		case 4: cout << a << b; break;
		case 5: cout << b; break;
		case 6: cout << b << a; break;
		case 7: cout << b << a << a; break;
		case 8: cout << b << a << a << a; break;
		case 9: cout << a << c; break;
		case 10: cout << c; break;
		default: cout << "Error"; break;
	} return;
}
