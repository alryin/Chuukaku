#include <iostream>
#include "lcs.h"

using namespace std;

int lcs_len(const char* x,const char* y,const int xl,const int yl, int** b){
    int c[xl+1][yl+1];
    for (int i = 0; i<4; i++) {
        c[i][0] = 0;
    }
    for (int j = 1; j<4; j++) {
        c[0][j] = 0;
    }
    for (int i = 1; i < xl+1; i++){
        for (int j = 1; j < yl+1; j++){
            if(x[i-1] == y[j-1]) {
                c[i][j] = c[i-1][j-1] + 1;
                b[i][j] = 1;
            } else {
                if(c[i-1][j] > c[i][j-1]){
                    c[i][j] = c[i-1][j];
                    b[i][j] = 2;
                } else{
                    c[i][j] = c[i][j-1];
                    b[i][j] = 3;
                }
            } 
        }
        
    }
    int r = c[xl][yl];
    return r;
}

void print_lcs(int** b, const char* x, int i, int j){
    if (i < 0 || j < 0) return;
    if (b[i+1][j+1] == 1){
        print_lcs(b, x, i-1, j-1);
        cout << x[i];
    } else if (b[i+1][j+1] == 2){
        print_lcs(b, x, i-1, j);
    } else {
        print_lcs(b, x, i, j-1);
    }
}
