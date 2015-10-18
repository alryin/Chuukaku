#include <iostream>
#include "nsqrt.h"

int main(int argc, char *argv[]){
	
	double x;
	double epsilon;
	
	//std::cin.good() --> verifica che l'utente inserisca un
	//valore corretto con quello della variabile
	
	std::cin.exceptions(std::istream::failbit); //lancia le eccezioni in caso di conversione mal riuscita
	
	try{	
		std::cout << "Inserire il valore x: ";
		std::cin >> x;
	
		std::cout << "Inserire il valore epsilon: ";
		std::cin >> epsilon;
	}catch(...){
		std::cout << "Inserire un numero!!!!!" << std::endl;
		return 0;
	}
	std::cout << "Il valore della radice quadrata e': " 
					<< nsqrt(x, epsilon) << std::endl;
	
	return 0;
}
