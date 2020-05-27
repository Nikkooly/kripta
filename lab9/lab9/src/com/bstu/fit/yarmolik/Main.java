package com.bstu.fit.yarmolik;

import java.util.Arrays;
import java.util.Scanner;

public class Main {

    public static final Scanner SCANNER = new Scanner(System.in);
    public static final Print print = new Print();

    public static void main(String[] args) {
        print.print("Введите текст: ");
        String text = SCANNER.nextLine();

        byte[] bytes = text.getBytes();
        print.println("Байты текста: " + Arrays.toString(bytes));

        Cryptography cryptography = new Cryptography(8);

        int[] encryptedBytes = cryptography.encrypt(bytes);
        print.println("Зашифрованные байты: " + Arrays.toString(encryptedBytes));

        byte[] decryptedBytes = cryptography.decrypt(encryptedBytes);
        print.println("Расшифрованные байты: " + Arrays.toString(decryptedBytes));

        String decryptedString = new String(decryptedBytes);
        print.println("Исходный текст: " + decryptedString);
    }
}
