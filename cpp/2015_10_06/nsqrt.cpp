#include "nsqrt.h"
#include <cmath>

double nsqrt(double x, double epsilon){
	double s = x/2;
	
	while(std::fabs(s*s-x) > epsilon){
		s = (s*s+x)/(2*s);
	}
	
	return s;
}
