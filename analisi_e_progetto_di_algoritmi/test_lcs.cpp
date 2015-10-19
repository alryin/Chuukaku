#include <iostream>
#include "lcs.h"

int main(int argc, char* argv[]){
	if (argc!=3){
		std::cout << "No parameters" << std::endl;
		return 0;
	}
	
	int xl = 0; 
	int yl = 0;
	while(*argv[1] != '\0') {
		xl++; 
		*argv[1]++;
	}
	while(*argv[2] != '\0') {
		yl++; 
		*argv[2]++; 
	}
		
	std::cout << "the ric answer is: " << lcs_len_ric(argv[1], argv[2], xl, yl) << std::endl;
	std::cout << "the iter answer is: " << lcs_len(argv[1], argv[2], xl, yl) << std::endl;
	return 0;
}
