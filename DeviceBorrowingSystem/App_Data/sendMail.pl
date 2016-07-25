#!/usr/bin/perl -w

# module declaration

use Net::SMTP_auth;
use MIME::Lite;

#get'the smtp server host. 

$mailhost="WHQBEMAIL2.whq.wistron";

#Information to different project.

$username="johnson_ting"; # prefix of your own mail ex: johnson_ting@wistron.com
$passwd="XXXXXXXX"; # WHQ domain password
@destaddress = ('johnson_ting@wistron.com', 'johnson_ting@wistron.com'); # if more than one mail address, you can use the ¡§,¡¨ to seperate each mail address 

$subject="[Butterfly] daily build report "; # write the mail¡¦s subject here
# @attachment = ('sendMail.pl', 'sendMail.pl'); # what attached files are needed to send. ex:/home/marco/boot.sh
@attachment = ($ARGV[1]); # specify the file from the command line input parameter
$content="Refer to attached files"; # write the mail content here

# append daily build result on the end of subject
$subject.=$ARGV[0];
# append daily build result on the end of content
$content.="\n";
$content.="\n";
$content.=$ARGV[2];
$content.="\n";
$content.="\n";
$content.=$ARGV[3];

#connect with SMTP server

$smtp = Net::SMTP_auth->new($mailhost) || die "Can't connect to SMTP Server";
print "The SMTP server is:".$smtp->banner()."\n";

#generate your source address.

$saddress=$username."\@" ;
$saddress.="wistron.com";
$saddress=~s/@/\@/g;#regex expression, for example : xiaoping4220\@163.com.

#authentication 

$num=$smtp->auth ('LOGIN',$username,$passwd);

foreach my $destaddress (@destaddress)
{

	# Create a new multipart message:
	my $msg = MIME::Lite->new(
	    From    => $saddress,
	    To      => $destaddress,
	    Subject => $subject,
	    Type    =>'multipart/mixed',
	    )or print "Error creating MIME body: $!\n";

	# Add parts:
	$msg->attach(Type     =>'TEXT',
		     Data    => $content,
		    );
	foreach my $attachment (@attachment)
	{
	    $msg->attach(
	      Type     => 'AUTO',      # the attachment mime type
	      Path     => $attachment, # local address of the attachment
	      )or print "Error attaching test file: $!\n";
	}

	my $str = $msg->as_string() or print "Convert the message as a string: $!\n";

	# Send the From and Recipient for the mail servers that require it
	$smtp->mail($saddress);
	$smtp->to($destaddress);

	# Start the mail
	$smtp->data();

	# Send the message
	$smtp->datasend("$str");

	# Send the termination string
	$smtp->dataend();
}
$smtp->quit;


#------------------------------------------------------------------------------

#$smtp->mail($saddress);
#$smtp->to($destaddress);
#mail data department.

#$smtp->data();
#$smtp->datasend("From: $saddress \n");
#$smtp->datasend("To: $destaddress \n");
#$smtp->datasend("Subject: perl mail test\n");
#$smtp->datasend("\n");
#$smtp->datasend("This is xiaoping's test email \n");
#$smtp->datasend("Have a good day!\n");
#$smtp->dataend();
#$smtp->quit;

print "The mail has been sent successful!\n";
exit 0;