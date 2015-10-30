#include <iostream>
#include "partition.h"

using namespace std;

int main(int argc, char* argv[]){
    
    cout << "Enter length of sequence" << endl;
    
    int temp = 0;
    cin >> temp;
    if (temp < 1) return 0;
    if (temp == 1) {
        cout << "the answer is: false" << endl;
        return 0;
    }
    
    const int len = temp;
    
    cout << "Enter the sequence" << endl;
    
    int seq[len];
    
    for (int i = 0; i < len; i++){ 
        cin >> seq[i];
        if (seq[i] < 0) 
            return 0;
        cout << "seq["<< i <<"]: " <<  seq[i] << endl;
    }
    if (len == 2){
        if (seq[0] == seq[1])
            cout << "the answer is: " << boolalpha << true << endl;
        else
            cout << "the answer is: " << boolalpha << false << endl;
        return 0;
    }
        
    /* 
     boolalpha stampa a video true/false quando si fa il cout di un valore booleano;
     in caso contrario stampa interi (0 o 1).
     
     http://stackoverflow.com/questions/8261674/c-bool-returns-0-1-instead-of-true-false
    */
    cout << "the answer is: " << boolalpha << partition(seq, len) << endl; 

    return 0;
}
