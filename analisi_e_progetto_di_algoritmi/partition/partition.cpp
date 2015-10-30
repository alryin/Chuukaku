#include <iostream>
#include "partition.h"

using namespace std;

bool partition(const int* seq, const int n){
    
    
    int t = 0;
    for (int i = 0; i < n; i++){
        t = t + seq[i];
    }
        
    if (t%2 != 0) return false; // se t è dispari non posso dividere equamente
    
    const int t2 = t/2; // t2 è la lunghezza della matrice
    
    bool m[n+1][t2];
    
    for (int i = 0; i < t2; i++){
        m[0][i] = false;
    }
    
    for (int i = 0; i < n + 1; i++){
        for (int j = 0; j < t2; j++){
            
            if (seq[i]==j)
                m[i][j] = true;
            else if (m[i-1][j] || m[i-1][j-seq[i]]) 
                m[i][j] = true;
            else
                m[i][j] = false;
        }
        if (m[i][t2-1]) return true;
    }
    if (m[n][t2-1])
        return true;
    return false;
}
