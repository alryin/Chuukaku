#include <iostream>
#include "avector.h"
//#include "pair.h"

struct compare_int{
	bool operator()(int a, int b) const{
		return a==b;
	}
	
};
int main(){
	
	avector<int, int, compare_int> av;
	
	try{
		av.add(10, 1.0);
		av.add(11, 1.1);
		av.add(12, 1.2);
		av.add(10, 2.2);
	}
	catch(duplicated_key &e){
		std::cout << e.what() << std::endl;
	}
	
	return 0;
}
