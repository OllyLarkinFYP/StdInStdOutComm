# StdInStdOutComm

The communication protocol that this package handles is defined below:

HEADER: 4 bytes: defines the size of the following message  
BODY: n bytes: the message to be sent - size should match the size defined in the header

This package will handle sending and receiving messages using this protocol over stdin/out.

Library code can be found in `StdInStdOutComm/src/StdInStdOutCommLibrary`.
