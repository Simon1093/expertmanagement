!Menu/Build/Settings/Fortran/General/Debug information:[None *]

SUBROUTINE cr(n,m,z,v,w)
!MS$ATTRIBUTES STDCALL,ALIAS:'_cr@20'::cr
IMPLICIT NONE
INTEGER,INTENT(IN)::n,m
REAL,INTENT(IN)::z(1:n,1:n)
REAL,INTENT(INOUT)::v(1:n),w(1:n,1:n)
REAL,ALLOCATABLE::a(:,:),b(:,:),x(:),y(:)
INTEGER i,j
REAL c,d/1.0/,e,o
ALLOCATE(a(1:n,1:n),b(1:n,1:n),x(1:n),y(1:n))
w=-z*d
DO i=1,n
 w(i,i)=w(i,i)+1.0
ENDDO
a=MATMUL(TRANSPOSE(w),w)
x=1.0/SQRT(REAL(n)); e=0.0; o=1.0
DO WHILE(ABS(e-o)/(ABS(e)+ABS(o)).GE.1.0E-07)!E-06)
 o=e
 y=MATMUL(a,x)
 e=DOT_PRODUCT(y,x)
 x=y/SQRT(DOT_PRODUCT(y,y))
ENDDO
WRITE(*,*)e
!READ(*,*)
b=-a
DO i=1,n
 b(i,i)=b(i,i)+e
ENDDO
v(n)=1.0/e; w(:,n)=x
DO j=1,m
 y=1.0; e=0.0; o=1.0
 DO i=1,j-1
  c=DOT_PRODUCT(y,w(:,i))/DOT_PRODUCT(w(:,i),w(:,i))
  y=y-c*w(:,i)
 ENDDO
 x=y/SQRT(DOT_PRODUCT(y,y))
 DO WHILE(ABS(e-o)/(ABS(e)+ABS(o)).GE.1.0E-07)!E-06)
  DO i=1,j
   o=e
   y=MATMUL(b,x)
   x=y/SQRT(DOT_PRODUCT(y,y))
   e=DOT_PRODUCT(MATMUL(a,x),x)
  ENDDO
  DO i=1,j-1
   c=DOT_PRODUCT(x,w(:,i))/DOT_PRODUCT(w(:,i),w(:,i))
   x=x-c*w(:,i)
  ENDDO
 ENDDO
 WRITE(*,*)e,j
! READ(*,*)
 v(j)=e; w(:,j)=x
ENDDO
DEALLOCATE(y,x,b,a)
END SUBROUTINE cr
