#Decryptor.Net

Decryptor.Net is a web application built on the ASP.Net MVC5 technology stack. It allows for the encryption and decryption of text files using the Vigenere algorithm. The application features an interactive interface designed to provide a convenient and comfortable user experience.

With Decryptor.Net, users can manually enter text or copy it from the clipboard in the "Encrypt Text" and "Decrypt Text" tabs. Alternatively, they can upload text files in .txt and .docx formats in the "Encrypt File" and "Decrypt File" tabs.

The application also provides the ability to customize the encryption alphabet. Users can choose from pre-defined character sets or create their own. The encryption alphabet is checked for duplicate characters, and all characters in the alphabet must be unique. While it is technically possible to select an empty encryption alphabet, the user will be warned about the incorrect input of the key (the key can only consist of characters from the encryption alphabet, and if it is empty, no non-empty key will pass the validation).

During the encryption/decryption process, the case of the text is preserved. If the input file contains uppercase letters, they will remain uppercase in the output. However, the key is case-insensitive. The keys "Scorpion" and "scorpion" are considered equivalent.

The output of any encryption algorithm in the program is displayed in the corresponding field, and users are also provided with the option to download the result in .txt and .docx formats. By default, both input and output files are stored in the App_Data folder, but the contents of this folder are cleared before new files are generated for download.

The program's functionality is covered by unit tests using the built-in Visual Studio toolkit.

I hope this summary accurately describes the Decryptor.Net project based on the provided ReadMe file. Let me know if you have any further questions!
