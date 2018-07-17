//Menu/Project/Settings.../Link/Customize/[ ]Use Program Database
//Menu/Project/Files.../Add [cr.obj, CONSOLE.LIB, LIBF.LIB, PORTLIB.LIB]
extern void __stdcall cr(long n, long m, float*a, float*v, float*w);

#include<stdio.h>
#include<malloc.h>
void main(void)
{
 FILE*f;
 int i,j,n;
 float*a,*v,*w;
 f=(FILE*)fopen("a_matrix.txt","r");
 i=(int)fscanf(f,"%d",&n);
 a=calloc(n*n,sizeof(*a));
 for(i=0;i<n;i++)for(j=0;j<n;j++)(int)fscanf(f,"%f",&a[n*j+i]);
 (int)fclose(f);
 v=calloc(n,sizeof(*v));
 w=calloc(n*n,sizeof(*w));
 j=n-1;
 cr(n,j,a,v,w);
 (int)printf("\nPress Enter");
 (int)getchar();
 free(w); free(v); free(a);
}
