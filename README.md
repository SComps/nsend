# nsend

Command line:  nsend <ip address> <port>

If you don't pipe or redirect into this, you can type whatever you want, and finish with a Conrol-Z  (^Z)

Examples:

<b>echo "lc 12,4,jobs/myjob.job | nsend 127.0.0.1 6662</b>         (for dtcyber with NOS 2.7.8)

you can also:

<b>nsend 127.0.0.1 3505 < myjob.jcl</b>        (to send a job to hercules)

This program doesn't care what it's sending, or where it's sending it to.  These are just examples.

The program and it's source code are free for anyone to use, however they choose.
