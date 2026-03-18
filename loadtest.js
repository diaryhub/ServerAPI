import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    vus: 50,
    duration: '30s',
};

// 각 VU(가상 유저)가 자신의 토큰을 캐싱해둘 변수
let userToken = ''; 

export default function () {
    // [핵심] exec 모듈 대신 k6 내장 전역 변수 __VU를 사용합니다. (1 ~ 50 할당)
    let userId = __VU; 

    // 토큰이 없다면 해당 유저 번호로 최초 1회 로그인 수행
    if (!userToken) {
        let authUrl = 'http://localhost:5098/api/auth/login'; // 실제 포트 번호로 변경
        let authPayload = JSON.stringify({ userId: userId });
        let authParams = { headers: { 'Content-Type': 'application/json' } };
        
        let authRes = http.post(authUrl, authPayload, authParams);
        
        // 간혹 로그인이 실패할 경우를 대비한 방어 로직
        if (authRes.status === 200) {
            userToken = authRes.json('token');
        } else {
            console.error(`VU ${userId} 로그인 실패`);
            return; 
        }
    }

    // 발급받은 개별 토큰으로 가챠 API 호출 (락 경합 분산)
    let gachaUrl = 'http://localhost:5098/api/gacha/draw'; // 실제 포트 번호로 변경
    let gachaPayload = JSON.stringify({ bannerId: 1 });
    let gachaParams = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${userToken}`
        },
    };

    let res = http.post(gachaUrl, gachaPayload, gachaParams);

    check(res, {
        'is status 200': (r) => r.status === 200,
    });

    sleep(0.1);
}