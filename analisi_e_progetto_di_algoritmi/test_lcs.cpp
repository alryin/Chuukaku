#include <iostream>
#include "lcs.h"

using namespace std;

int main(int argc, char* argv[]){
    
    int a = 0; 
    int c = 0;
    
    cout << "Enter the length of string x" << endl;
    cin >> a;
    const int n = a;
    char x[n];
   
    cout << "Enter the string x" << endl;
    for(int i=0; i<n; i++){ cin >> x[i]; }
    
    cout << "Enter the length of string y" << endl;
    cin >> c;
    const int m = c;
    char y[m];    
    
    cout << "Enter the string y" << endl;
    for(int i=0; i<m; i++){ cin >> y[i]; }
    
    int** b;
    b = new int*[n+1];
    
    for(int i = 0; i < n+1; i++){
        b[i] = new int[m+1];
    }
    
    cout << "the answer is: " << lcs_len(x, y, n, m, b) << endl;
    cout << "a Longest Common Substring is: " ;
    print_lcs(b, x, n - 1, m - 1); cout << endl;
    
    for(int i = 0; i<n; i++){
        delete(b[i]);
    } delete(b);
    
    return 0;
}