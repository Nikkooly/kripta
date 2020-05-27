
package com.bstu.fit.yarmolik;

import java.math.BigInteger;
import java.util.Arrays;
import java.util.Random;

public class Cryptography {

    public static final Random RND = new Random();
    public static final char ONE = '1';
    public static final char ZERO = '0';
    public static final int RADIX = 2;

    private int d[];
    private int e[];
    private int z;
    private int a;
    private int n;
    private int aInv;

    public Cryptography(int z) {
        this.z = z;
        d = new int[z];
        e = new int[z];
        init();
        printInfo();
        Arrays.sort(d);
    }

    private void init() {

        int sum = 0;
        int di;
        for (int i = 0; i < z; i++) {
            do {
                di = RND.nextInt(z) + sum;
            } while (di < sum);
            d[i] = ++di;
            sum += d[i];
        }

        a = RND.nextInt(sum);
        do {
            n = RND.nextInt(sum) + sum;
        } while (gcd(n, a) != 1);

        aInv = modInverse(a, n);

        for (int i = 0; i < z; i++) {
            e[i] = (d[i] * a) % n;
        }
    }


    public int[] encrypt(byte[] data) {
        int[] result = new int[data.length];
        String binaryCode;
        int sum;
        for (int i = 0; i < result.length; i++) {
            binaryCode = String.format("%8s",
                    Integer.toBinaryString(data[i])).replace(' ', '0');
            sum = 0;
            for (int j = 0; j < z; j++) {
                if (binaryCode.charAt(j) == ONE) {
                    sum += e[j];
                }
            }
            result[i] = sum;
        }
        return result;
    }

    public byte[] decrypt(int[] data) {
        StringBuilder binaryCode;
        int sum;
        byte[] result = new byte[data.length];
        for (int i = 0; i < result.length; i++) {
            sum = (data[i] * aInv) % n;
            binaryCode = new StringBuilder();
            for (int j = z - 1; j >= 0; j--) {
                if (d[j] <= sum) {
                    sum -= d[j];
                    binaryCode.append(ONE);
                } else {
                    binaryCode.append(ZERO);
                }
            }
            binaryCode.reverse().replace(0, 1, String.valueOf(ZERO));
            result[i] = (byte) Integer.parseInt(binaryCode.toString(), RADIX);
        }
        return result;
    }

    private int gcd(int a, int b) {
        return BigInteger.valueOf(a).gcd(BigInteger.valueOf(b)).intValue();
    }

    private int modInverse(int a, int m) {
        return BigInteger.valueOf(a)
                .modInverse(BigInteger.valueOf(m)).intValue();
    }

    void printInfo(){
        System.out.println("d: " + Arrays.toString(d));
        System.out.println("e: " + Arrays.toString(e));
        System.out.println("a = " + a);
        System.out.println("n = " + n);
        System.out.println("a^-1 = " + aInv);
    }
}
