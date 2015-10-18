#include <iostream> 
using namespace std;

 void stampa(const char* c){ while(*c != '\0'){cout << *c << endl; c++;} }	  

 int count(const char* c){
	 int a = 0;
	 while (*c != '\0'){
		 a++; c++;
	 } return a;
 }
 
void inverter(char* c){
	int a = count(c)-1, l = a;
	for(int i=0;i <(l/2);i++, a--){
		char seven = c[i];
		c[i] = c[a]; c[a] = seven;
	}
	stampa(c);
}
 
 char* copycat (const char *c){
	int l = count(c);
	char *copy = new char[l+1]; // <--	
	for (int i = 0; i <	l;i++){ copy[i] = c[i]; }
	copy[l] = '\0';
	return copy;
	
 }
 
 int main(int arc, char*zawarudo[]){
	if (arc!=2){
		cout << "No parameters bicth!" << endl;
		/*for(int i=0; ;){
			cout << "JUST DO IT";
		}*/
		return 0;
	}
	
	stampa(zawarudo[1]);
	cout << "the string length is: " << count(zawarudo[1]) << endl;
	char*copya = copycat(zawarudo[1]);
	//stampa(copy);
	inverter(copy);
	delete[] copya;//!!!
	return 0;
}
