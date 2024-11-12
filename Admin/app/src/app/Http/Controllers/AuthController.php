<?php

namespace App\Http\Controllers;

use App\Models\Account;
use Illuminate\Http\Request;

class AuthController extends Controller
{
    //
    public function login(Request $request)
    {
        return view('auth/loginview');
    }

    public function dologin(Request $request) //$requestの中に名前とパスワードの情報が入っている
    {
        if ($request['name'] === 'jobi' && $request['password'] === 'jobi') { //nameが'jobi',passwordが'jobi'だったら
            $request->session()->put('login', true);
            return redirect()->route('accounts.index'); //リダイレクト先(index)をルートで指定
            //以前のようにredirectで記入せず、->routeで指定する
        } else {
            echo '<p style="color: #FF2D20" >名前、またはパスワードが間違っています。</p>';
            return view('auth/loginview');//不一致だったらログイン画面を表示
        }
    }

    public function logout(Request $request)
    {
        return view('auth/loginview');
    }

    public function dologout(Request $request)
    {
        $request->session()->flush();
        $request->session()->forget('login');

        return redirect('auth/login');
    }
}
