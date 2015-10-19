#include <iostream>
#include "lcs.h"

int lcs_len(char* x, char* y, int xl, int yl){
	//std::cout << xl << " " << yl << std::endl;
	int c[xl+1][yl+1];
	
		x[0]='a'; 
		y[0]='a';
		x[1]='c'; 
		y[1]='c';
		x[2]='a'; 
		y[2]='a';
		x[3]='b'; 
		y[3]='b';
	//std::cout << " " << std::endl;
	for (int i = 0; i<4/*xl*/; i++) {
		c[i][0] = 0;
	}
	for (int j = 1; j<4/*yl*/; j++) {
		c[0][j] = 0;
	}
	
	for (int i = 1; i < xl+1; i++){
		for (int j = 1; j < yl+1; j++){
			std::cout << x[i-1] << " " << y[j-1] << std::endl;
			if(x[i-1] == y[j-1]) {
				c[i][j] = c[i-1][j-1] + 1;
				std::cout << "a" << " " << i << " " << j << std::endl;
				std::cout <<"risposta temporanea a: " << c[i][j] << std::endl;		
			}
			else {
				int a = c[i-1][j];
				std::cout << "b" << " " << i << " " << j << std::endl;
				int b = c[i][j-1];
				std::cout << "c" << " " << i << " " << j << std::endl;
				if(a > b){
					c[i][j] = a;
					std::cout <<"risposta temporanea b: " << c[i][j] << std::endl;
				} 
					
				else{
					c[i][j] = b;
					std::cout <<"risposta temporanea: c " << c[i][j] << std::endl;
				}
			}
		}
	}
	int r = c[xl][yl];
	return r;
	
}

int lcs_len_ric(const char* x, const char* y, int i, int j){
	int a,b;
	if (i==0 || j==0) return 0;
	if (x[i] == x[j]) 
		return ( lcs_len_ric(x, y, i - 1,j - 1) + 1 );
	else
		a = lcs_len_ric(x, y, i,j - 1);
		b = lcs_len_ric(x, y, i - 1,j);
		if(a > b) return a;
		return b;
}
