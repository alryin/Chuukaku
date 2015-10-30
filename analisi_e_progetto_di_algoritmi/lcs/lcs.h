#ifndef LCS_H
#define LCS_H

    int lcs_len(const char* x,const char* y,const int xl,const int yl, int** b);
    void print_lcs(int** b, const char* x, int i, int j);
    
#endif
