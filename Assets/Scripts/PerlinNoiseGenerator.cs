using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator {

	private static readonly int[] permutation = { 151,160,137,91,90,15,                 // Hash lookup table as defined by Ken Perlin.  This is a randomly
    131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,    // arranged array of all numbers from 0-255 inclusive.
    190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
    88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
    77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
    102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
    135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
    5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
    223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
    129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
    251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
    49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
	};

	private static int[][] grad3 = new int[][] {new int[]{1,1,0},new int[]{-1,1,0},
												new int[]{1,-1,0},new int[]{-1,-1,0},
											  	new int[]{1,0,1},new int[]{-1,0,1},
												new int[]{1,0,-1},new int[]{-1,0,-1},
											  	new int[]{0,1,1},new int[]{0,-1,1},
												new int[]{0,1,-1},new int[]{0,-1,-1}};

	private static readonly int[] p;                                                    // Doubled permutation to avoid overflow
	private static int repeat = 0;

	static PerlinNoiseGenerator() {
		p = new int[512];
    	for(int x=0;x<512;x++) {
    		p[x] = permutation[x & 255];
    	}
	}

	public static double fade(double t) {
    	return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
	}

	public int inc(int num) {
		num++;
		if (repeat > 0) num %= repeat;
		return num;
	}

	public static double grad(int hash, double x, double y, double z)
	{
		switch(hash & 0xF)
		{
			case 0x0: return  x + y;
			case 0x1: return -x + y;
			case 0x2: return  x - y;
			case 0x3: return -x - y;
			case 0x4: return  x + z;
			case 0x5: return -x + z;
			case 0x6: return  x - z;
			case 0x7: return -x - z;
			case 0x8: return  y + z;
			case 0x9: return -y + z;
			case 0xA: return  y - z;
			case 0xB: return -y - z;
			case 0xC: return  y + x;
			case 0xD: return -y + z;
			case 0xE: return  y - x;
			case 0xF: return -y - z;
			default: return 0; // never happens
		}
	}

	public static double lerp(double a, double b, double x) {
		return a + x * (b - a);
	}
	
	public double perlin(double x, double y, double z) {
		if(repeat > 0) {                                    // If we have any repeat on, change the coordinates to their "local" repetitions
			x = x%repeat;
			y = y%repeat;
			z = z%repeat;
		}
		
		int xi = (int)x & 255;                              // Calculate the "unit cube" that the point asked will be located in
		int yi = (int)y & 255;                              // The left bound is ( |_x_|,|_y_|,|_z_| ) and the right bound is that
		int zi = (int)z & 255;                              // plus 1.  Next we calculate the location (from 0.0 to 1.0) in that cube.
		double xf = x-(int)x;
		double yf = y-(int)y;
		double zf = z-(int)z;

		double u = fade(xf);
		double v = fade(yf);
		double w = fade(zf);

		int aaa, aba, aab, abb, baa, bba, bab, bbb;
		aaa = p[p[p[    xi ]+    yi ]+    zi ];
		aba = p[p[p[    xi ]+inc(yi)]+    zi ];
		aab = p[p[p[    xi ]+    yi ]+inc(zi)];
		abb = p[p[p[    xi ]+inc(yi)]+inc(zi)];
		baa = p[p[p[inc(xi)]+    yi ]+    zi ];
		bba = p[p[p[inc(xi)]+inc(yi)]+    zi ];
		bab = p[p[p[inc(xi)]+    yi ]+inc(zi)];
		bbb = p[p[p[inc(xi)]+inc(yi)]+inc(zi)];
	
		double x1, x2, y1, y2;
		x1 = lerp(    grad (aaa, xf  , yf  , zf),           // The gradient function calculates the dot product between a pseudorandom
					grad (baa, xf-1, yf  , zf),             // gradient vector and the vector from the input coordinate to the 8
					u);                                     // surrounding points in its unit cube.
		x2 = lerp(    grad (aba, xf  , yf-1, zf),           // This is all then lerped together as a sort of weighted average based on the faded (u,v,w)
					grad (bba, xf-1, yf-1, zf),             // values we made earlier.
					u);
		y1 = lerp(x1, x2, v);

		x1 = lerp(    grad (aab, xf  , yf  , zf-1),
					grad (bab, xf-1, yf  , zf-1),
					u);
		x2 = lerp(    grad (abb, xf  , yf-1, zf-1),
					grad (bbb, xf-1, yf-1, zf-1),
					u);
		y2 = lerp (x1, x2, v);
		
		return (lerp (y1, y2, w)+1)/2;                     
	}

	public double OctavePerlin(double x, double y, double z, int octaves, double persistence) {
		double total = 0;
		double frequency = 1;
		double amplitude = 1;
		double maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
		for(int i=0;i<octaves;i++) {
			total += perlin(x * frequency, y * frequency, z * frequency) * amplitude;
			
			maxValue += amplitude;
			
			amplitude *= persistence;
			frequency *= 2;
		}
		
		return total/maxValue;
	}

	private static double dot(int[] g, double x, double y, double z) {
 		return g[0]*x + g[1]*y + g[2]*z; 
	}

	public double simplexNoise(double x, double y, double z){
		double n0, n1, n2, n3;
		double skewFactor = (x + y + z) / 3.0f; // Simple skew factor for 3D
		// Find the simplex cell we are in
		int i = (int) (x + skewFactor);
		int j = (int) (y + skewFactor);
		int k = (int) (z + skewFactor);

		const double G = 1.0f / 6.0f;
		double t = (i + j + k) * G;
		// Unskew cell origin back to (x,y,z)
		double X0 = i - t;
		double Y0 = j - t;
		double Z0 = k - t;
		// x,y,z distances from cell origin
		double x0 = x - X0;
		double y0 = y - Y0;
		double z0 = z - Z0;

		//Determine the simplex we're in
		int i1, j1, k1; // Second corner of simplex
		int i2, j2, k2; // Third corner of simplex

		if(x0 >= y0){
			if(y0 >= z0){ i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0;} 
			else if(x0 >= z0){ i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1;} 
			else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1;}
		} else {
			if(y0 < z0){ i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1;} 
			else if(x0 < z0){ i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1;} 
			else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0;}
		}

		double x1 = x0 - i1 + G; // Offsets for second corner in (x,y,z) coords
		double y1 = y0 - j1 + G;
		double z1 = z0 - k1 + G;
		double x2 = x0 - i2 + 2.0f*G; // Offsets for third corner in (x,y,z) coords
		double y2 = y0 - j2 + 2.0f*G;
		double z2 = z0 - k2 + 2.0f*G;
		double x3 = x0 - 1.0f + 3.0f*G; // Offsets for last corner in (x,y,z) coords
		double y3 = y0 - 1.0f + 3.0f*G;
		double z3 = z0 - 1.0f + 3.0f*G;

		int ii = i & 255;
		int jj = j & 255;
		int kk = k & 255;
		int gi0 = p[ii+p[jj+p[kk]]] % 12;
		int gi1 = p[ii+i1+p[jj+j1+p[kk+k1]]] % 12;
		int gi2 = p[ii+i2+p[jj+j2+p[kk+k2]]] % 12;
		int gi3 = p[ii+1+p[jj+1+p[kk+1]]] % 12;

		double t0 = 0.5 - x0*x0 - y0*y0 - z0*z0;
		if(t0<0) n0 = 0.0;
		else {
			t0 *= t0;
			n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
		}
		double t1 = 0.5 - x1*x1 - y1*y1 - z1*z1;
		if(t1<0) n1 = 0.0;
		else {
			t1 *= t1;
			n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
		}
		double t2 = 0.5 - x2*x2 - y2*y2 - z2*z2;
		if(t2<0) n2 = 0.0;
		else {
			t2 *= t2;
			n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
		}
		double t3 = 0.5 - x3*x3 - y3*y3 - z3*z3;
		if(t3<0) n3 = 0.0;
		else {
			t3 *= t3;
			n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
		}
		// Add contributions from each corner to get the final noise value.
		// The result is scaled to stay just inside [-1,1]
		return 32.0*(n0 + n1 + n2 + n3);
	}
}
